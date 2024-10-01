import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Login from './Login';
import PrivateRoute from './PrivateRoute';
import OverviewInterface from './OverviewInterface';

const App = () => {
    const [isAuthenticated, setIsAuthenticated] = useState();

    // Check if the user is already authenticated (has a valid token in localStorage)
    useEffect(() => {
        const token = localStorage.getItem('token');
        console.log("Token: " + token);
        
        setIsAuthenticated(token != null)
        console.log(isAuthenticated)
    }, []);

    const handleSubmit = async (e, endpoint, method, postData, setData, isString = false) => {
        e.preventDefault();
        const url = `${baseUrl}${endpoint}`;
        const payload = JSON.stringify(postData);
        console.log(payload)
        try {
            const token = localStorage.getItem('token');
            const response = await fetch(url, {
                method: method,
                credentials: 'include',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: payload,
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const result = await response.text();
            return result ? JSON.parse(result) : {}
            //setResponseData(result ? JSON.parse(result) : 'Ok');
            //setData(isString ? '' : { id: '', is_admin: false, email: '', first_name: '', last_name: '', budgetId: '' });

        } catch (error) {
            console.error('Error:', error);
        }
    };

    // return (
    //     <Router>
    //         <Routes>
    //             {/* Sett innloggingssiden som standard rute */}
    //             <Route path="/" element={<Login setIsAuthenticated={setIsAuthenticated} />} />

    //             {/* Beskytt privat rute */}
    //             <Route
    //                 path="/overview"element={
    //                     <PrivateRoute isAuthenticated={isAuthenticated}>
    //                         <OverviewInterface setIsAuthenticated={setIsAuthenticated} isAuthenticated={isAuthenticated} />
    //                     </PrivateRoute>
    //                 }
    //             />

    //             {/* Omdiriger alle andre ruter til innlogging */}
    //             <Route path="*" element={<Navigate to="/" />} />
    //         </Routes>
    //     </Router>
    // );
    return (
        <>
            <Router>
                <Routes>
                    <Route element={<PrivateRoute isAuthenticated={isAuthenticated} />}>
                        <Route element={<OverviewInterface/>} path="/" exact/>
                    </Route>
                    <Route element={<Login setIsAuthenticated={setIsAuthenticated} isAuthenticated={{isAuthenticated}}/>} path="/login"/>
                </Routes>
            </Router>
        </>
    );

    
};

export default App;
