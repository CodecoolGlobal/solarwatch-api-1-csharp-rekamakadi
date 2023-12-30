import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import './LoginModal.css';

const LoginModal = ({ onLogin }) => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const handleLogin = async () => {
        try {
            // Send login request to the backend
            const response = await fetch.post('/api/login', {
                username,
                password,
            });

            // Assuming successful login if status is 200
            if (response.status === 200) {
                onLogin(); // Notify parent component about successful login
            }
        } catch (error) {
            console.error('Login failed', error);
            // Handle login failure
        }
    };

    return (
        <div className="login-container">
            <h2 className="login-heading">Login</h2>
            <input
                type="text"
                placeholder="Username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
            />
            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />
            <button onClick={handleLogin} className="login-button">
                Login
            </button>
            <Link to="/registration" className="register-link">
                <button className="login-button">Register</button>
            </Link>
        </div>
    );
};

export default LoginModal;