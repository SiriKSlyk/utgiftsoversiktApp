import React, { useState, useEffect } from 'react';

import Login from './components/Login';
import PrivateRoute from './components/PrivateRoute';
import Missing from './components/Missing'
import OverviewInterface from './components/OverviewInterface';
import { Routes, Route } from 'react-router-dom';
import Layout from './components/Layout';
import RequireAuth from './components/RequireAuth';

const App = () => {

    return (
        <Routes>
            <Route path="/" element={<Layout />}>
                <Route path="login" element={<Login />} />
                
                <Route element={<RequireAuth />}>
                    <Route path="/" element={<OverviewInterface />} />
                </Route>

                <Route path="*" element={<Missing />} />

            </Route>




        </Routes>
    );


};

export default App;
