const mongoose = require('mongoose');
const {Schema} = mongoose;

const questionsSchema = new Schema({
    question: { 
        type: String, 
        required: true 
    },
    options: {
        type: [{
            option: { 
                type: String, 
                required: true, 
                enum: ['A', 'B', 'C', 'D']
            },
            text: { 
                type: String, 
                required: true 
            }
        }],
        required: true
    },
    answer: { 
        type: String, 
        required: true, 
        enum: ['A', 'B', 'C', 'D'],
        
    }
});

const Question = mongoose.model('questions', questionsSchema);

module.exports = Question;