import "./Login.css"
import React, { useState } from 'react';
import axios from 'axios';
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { useNavigate } from 'react-router-dom';

const Login = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const navigate  = useNavigate();

    const handleSubmit = async (event) => {
        event.preventDefault();
    
        const apiUrl = 'https://localhost:7136/auth/login';
    
        try {
            const response = await axios.post(apiUrl, {
                username: username,
                password: password,
            });

            if (response.status === 200) {
                const token = response.data.token;
                localStorage.setItem('token', token);
                toast('Login was successful!');
                navigate('/dashboard');
            }
        }
        catch (error) {
            if (error.response) {
                toast(error.response.data);
            } else if (error.request) {
                toast('No response from the server. Please try again later.');
            } else {
                toast('An unexpected error occurred. Please try again later.');
            }
        };
    }
    return (
        <div className="login">
            <h1>Login</h1>
            <form onSubmit={handleSubmit}>
                <input type={"text"} placeholder={"Username"} value={username} onChange={(e) => setUsername(e.target.value)}/>
                <input type={"password"} placeholder={"Password"} value={password} onChange={(e) => setPassword(e.target.value)}/>
                <button type={"submit"}>Login</button>
            </form>
        </div>
    )
}

export default Login;