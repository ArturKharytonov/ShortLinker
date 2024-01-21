import "./SignUp.css";
import React, { useState } from 'react';
import axios from 'axios';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

const Signup = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const handleSignup = async (event) => {
        event.preventDefault();

        const apiUrl = 'https://localhost:7136/auth/register';

        try {
            const response = await axios.post(apiUrl, {
                username: username,
                password: password,
            });

            if (response.status === 200) {
                toast('Signup successful, u have to login now!');
                console.log('Успішна реєстрація:', response.data);
            }
            else {
                //const errorMessage = response.data && response.data.error;
                toast('User with this username already exist');
                console.error('Помилка реєстрації:', response.status, response.statusText);
            }
        }
        catch (error) {
            console.error('Помилка відправки запиту:', error.message);
        }
    };

    return (
        <div className="signup">
            <h1>Sign Up</h1>
            <form onSubmit={handleSignup}>
                <input type={"text"} placeholder={"Username"} value={username} onChange={(e) => setUsername(e.target.value)}/>
                <input type={"password"} placeholder={"Password"} value={password} onChange={(e) => setPassword(e.target.value)}/>
                <button type={"submit"}>Sign Up</button>
            </form>
        </div>
    )
}

export default Signup;