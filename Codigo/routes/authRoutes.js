
const express = require('express');

const Question = require('../model/questions');

module.exports = app => {

    app.use(express.json());
    app.use(express.urlencoded({extended: true}));
    
    app.post('/questions/create', async (req, res) => {
        try {
            var responsePreg = {};

            let { rQuestion, rOptions, rAnswer } = req.body;
    
            // Validar que la pregunta no esté vacía
            if (!rQuestion || rQuestion.trim() === "") {
                responsePreg.code = 2;
                responsePreg.msg = "La pregunta no puede estar vacía.";
                console.log("La pregunta no puede estar vacía.");
                return res.send(responsePreg);
            }
    
            // Validar que rOptions sea un array de 4 opciones
            if (!Array.isArray(rOptions) || rOptions.length !== 4) {
                responsePreg.code = 3;
                responsePreg.msg = "Debe haber 4 opciones.";
                console.log("Debe haber 4 opciones.");
                return res.send(responsePreg);
            }
    
            // Validar que cada opción tenga `option` (A, B, C, D) y `text`
            const validOptions = ['A', 'B', 'C', 'D'];
            for (let opt of rOptions) {
                if (!opt.option || !opt.text || !validOptions.includes(opt.option)) {
                    responsePreg.code = 4;
                    responsePreg.msg = "Cada opción debe tener una letra (A-D) y un texto.";
                    console.log("Cada opción debe tener una letra (A-D) y un texto.");
                    return res.send(responsePreg);
                }
            }
    
            // Validar que `rAnswer` sea una de las opciones
            if (!validOptions.includes(rAnswer) || !rOptions.some(opt => opt.option === rAnswer)) {
                responsePreg.code = 5;
                responsePreg.msg = "La respuesta debe coincidir con una de las opciones proporcionadas.";
                console.log("La respuesta debe coincidir con una de las opciones proporcionadas.");
                return res.send(responsePreg);
            }
    
            // Verificar si la pregunta ya existe en la base de datos
            const existingQuestion = await Question.findOne({ question: rQuestion });
            if (existingQuestion) {
                responsePreg.code = 1;
                responsePreg.msg = "Esta pregunta ya está registrada.";
                console.log("Esta pregunta ya está registrada.");
                return res.send(responsePreg);
            }
    
            // Guardar la nueva pregunta en la base de datos
            const newQuestion = new Question({ question: rQuestion, options: rOptions, answer: rAnswer });
            await newQuestion.save();
    
            responsePreg.code = 0;
            responsePreg.msg = "Pregunta guardada correctamente."
            responsePreg.data = { id: newQuestion._id, question: newQuestion.question };
            console.log("Pregunta guardada correctamente.");
            return res.send(responsePreg);
    
        } catch (error) {
            console.log("Error procesando la pregunta", error);
            res.status(500).send('Internal Server Error');
        }
    });

    app.delete('/questions/delete/:id', async (req, res) => {
        try {
            var responsePreg = {};

            const { id } = req.params;
            const deletedQuestion = await Question.findByIdAndDelete(id);
    
            if (!deletedQuestion) {
                return res.status(404).send(responsePreg={ code: 6, msg: "Pregunta no encontrada." });
            }

            console.log("Pregunta eliminada correctamente.");
            res.send(responsePreg={ code: 7, msg: "Pregunta eliminada correctamente." });
        } catch (error) {
            console.error("Error eliminando la pregunta:", error);
            res.status(500).send(responsePreg={ code: 8, msg: "Error interno del servidor." });
        }
    });  


}