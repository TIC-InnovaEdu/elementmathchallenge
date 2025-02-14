const mongoose = require('mongoose');
const Account = mongoose.model('accounts');
const argon2 = require('argon2');
const crypto = require('crypto');

module.exports = app => {

    //Routes
    app.post('/account/login', async (req, res) =>{

        try {
            var response = {};

            const {rUsername, rPassword} = req.body;
            if (rUsername === null || rPassword == null) {
                response.code = 1;
                response.msg = "Invalid Credentials";
                return res.send(response); // Usar return para evitar continuar

            }

            var userAccount = await Account.findOne({username: rUsername}, 'username adminFlag password');
            console.log(userAccount);
            if (userAccount != null) {
                argon2.verify(userAccount.password, rPassword).then(async (success) => {
                    if (success) {
                        userAccount.lastAuthentication = Date.now();
                        await userAccount.save();

                        response.code = 0;
                        response.msg = "Account found";
                        response.data = (({username, adminFlag}) => ({username, adminFlag}))(userAccount);

                        return res.send(response);
                    }else{
                        response.code = 1;
                        response.msg = "Invalid Credentials";
                        return res.send(response);
                    }
                });
            }else{
                response.code = 1;
                response.msg = "Invalid Credentials";
                return res.send(response);
            }
            
        } catch (error) {
            console.log(error);
            res.status(500).send({message: 'Error interno del serviro'})
        }
            
    });

    app.post('/account/create', async (req, res) =>{

        try {
            var response = {};

            const {rUsername, rPassword} = req.body;
        if (rUsername === null || rPassword == null) {
            response.code = 1;
            response.msg = "Invalid Credentials";
            return res.send(response);
        }

        var userAccount = await Account.findOne({username: rUsername}, '_id');
        if (userAccount == null) {
            // Create a new account
            console.log("Create a new account...");
            
            //Generate a unique access token
            crypto.randomBytes(32, function(err, salt){
                if (err) {
                    console.log(err);
                }
                //accountSalt = salt;
                argon2.hash(rPassword, salt).then(async (hash) =>{
                    var newAccount = new Account({
                        username: rUsername,
                        password: hash,
                        salt: salt,
        
                        lastAuthentication: Date.now() 
                    });
                    await newAccount.save();

                    response.code = 0;
                    response.msg = "Account found";
                    response.data = (({username}) => ({username}))(newAccount);

                    return res.send(response);

                    //hashedPassword = hash;
                });
            });
            

            /*res.send(newAccount);
            return;*/
            // Enviar la nueva cuenta como respuesta
        }else{
            //res.send("Username is already taken");
            response.code = 2;
            response.msg = "Username is already taken";
            return res.send(response);
        }
        
        /*res.send('Invalid Credentials');
        return;*/
        } catch (error) {
            console.log(error);
            // Manejar errores y enviar respuesta
            res.status(500).send('Internal Server Error');
        }
        
            
    });
}

