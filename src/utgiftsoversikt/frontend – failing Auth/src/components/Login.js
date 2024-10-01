
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Login.css';

const Login = ({
    setIsAuthenticated,
    isAuthenticated



}) => {
    const navigate = useNavigate();

    const baseUrl = 'https://localhost:7062/'

    const [data, setData] = useState([]);
    const [loginData, setLoginData] = useState('');

    const handleLogin = () => {
        // Oppdaterer autentiseringstilstanden
        setIsAuthenticated(true);
        navigate('/'); // Omdiriger til oversiktssiden etter innlogging
    };

    const loginSubmit = async (e) => {
        e.preventDefault();
        const url = `${baseUrl}auth/login`; // Pass på at URL-en er korrekt
        const payload = JSON.stringify(loginData); // Bruker loginData-objektet
        console.log("Payload: " + payload);

        

        try {
            const response = await fetch(url, {
                method: "POST",
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                },
                body: payload,
                credentials: 'include',
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();
            const token = data.token;

            console.log(loginData);
            console.log(token);
            

            setIsAuthenticated(false);
            if (token != null) {
                localStorage.setItem('token', token); // Lagre token i localStorage
                handleLogin(); // Kall handleLogin for å oppdatere tilstand og omdirigere
                setIsAuthenticated(true);
            } else {
                console.error("No token received");
                
            }
            console.log("IsAuth: " + isAuthenticated)

            console.log(data);
        } catch (error) {
            console.error('Error:', error);
            
        }
        
    };

    return (
        <>


            <div className="grid-item">
                <h2>Login</h2>
                <form onSubmit={(e) => loginSubmit(e, 'auth/login', "POST", loginData, setLoginData, true)}>
                    <input
                        type="text"
                        name="mail"
                        placeholder="mail"
                        value={loginData.mail}
                        onChange={(e) => { setLoginData(e.target.value); }}
                    />
                    <button type="submit">Send</button>
                </form>
            </div>


        </>
    );
};

export default Login;
