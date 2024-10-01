
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './Login.css';

const Logout = ({
  setIsAuthenticated,



}) => {
  const navigate = useNavigate();

  const baseUrl = 'https://localhost:7062/'

  const [data, setData] = useState([]);
  const [logoutData, setLogoutData] = useState('');

  const handleLogout = () => {
    // Simulate authentication (you'd typically handle this with an API call)
    setIsAuthenticated(false);
    navigate('/login'); // Redirect to your dashboard or main content page after login
  };

  const logoutSubmit = async (e, endpoint, method, postData, setData, isString = false) => {
    e.preventDefault();
    const url = `${baseUrl}${endpoint}`;
    const payload = JSON.stringify(postData);
    console.log(payload)
    const token = localStorage.getItem('token')
    try {
      const response = await fetch(url, {
        method: method,
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,
        },
        credentials: 'include',

      });

      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      const data = await response.json();

      
      localStorage.setItem('token', null);
      console.log("Loged out token: " + localStorage.getItem('token'))
      handleLogout()


      //setResponseData(data ? JSON.parse(data) : 'Ok');
      //setData(isString ? '' : data);
      console.log(data);

    } catch (error) {
      console.error('Error:', error);
    }
    navigate('/login'); // Redirect to your dashboard or main content page after login
  };

  return (
    <>
      {/*<div className="grid-item">
          <h2>Logout</h2>
          <form onSubmit={(e) => loginSubmit(e, 'auth/logout', "POST", loginData, setLoginData, true)}>
            <button type="submit">Send</button>
          </form>
        </div>*/}
      <form onSubmit={(e) => logoutSubmit(e, 'auth/logout', "POST", logoutData, setLogoutData, true)}>
        <button type="submit">Send</button>
      </form>
    </>
  );
};

export default Logout;
