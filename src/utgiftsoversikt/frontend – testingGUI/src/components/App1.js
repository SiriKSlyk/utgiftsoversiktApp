import React, { useState } from 'react';
import './App.css'; // Legg til en CSS-fil for styling

const App = () => {
  const [data, setData] = useState([]);
  const [getUser, setGetUser] = useState('');
  const [createUser, setCreateUser] = useState({ id: '', is_admin: false, email: '', first_name: '', last_name: '', budgetId: '' });
  const [updateUser, setUpdateUser] = useState({ id: '', is_admin: false, email: '', first_name: '', last_name: '', budgetId: '' });
  const [deleteUser, setDeleteUser] = useState('');
  const [responseData, setResponseData] = useState(null); // Legg til en tilstand for å lagre responsen
  const baseUrl = 'https://utgiftsoversikt.livelyriver-2c344957.northeurope.azurecontainerapps.io/';

  const fetchData = async () => {
    try {
      const response = await fetch(`${baseUrl}users/getall`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({}),
      });
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      const result = await response.json();
      setData(result);
    } catch (error) {
      console.error('Error fetching data:', error);
      alert('Error fetching data. Check console for details.');
    }
  };

  const handleInputChange = (e, setPostData) => {
    const { name, value, type, checked } = e.target;
    setPostData(prevData => ({
      ...prevData,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleSubmit = async (e, endpoint, method, postData, setData, isString = false) => {
    e.preventDefault();
    const url = `${baseUrl}${endpoint}`;
    const payload = isString ? JSON.stringify(postData) : JSON.stringify(postData);
    console.log(`Posting data to: ${url}`, payload); // Log payload to verify it's correct

    try {
      const response = await fetch(url, {
        method: method,
        headers: {
          'accept': 'text/plain',
          'Content-Type': 'application/json',
        },
        body: payload,
      });
      console.log('Full response:', response); // Log the full response

      if (!response.ok) {
        const errorText = await response.text();
        console.error('Error response from server:', errorText);
        throw new Error(`Network response was not ok: ${response.status} ${response.statusText}`);
      }

      let result;
      const text = await response.text();
      if (text) {
        try {
          result = JSON.parse(text);
        } catch (error) {
          console.error('Error parsing JSON:', error);
          result = text; // Fallback to plain text if JSON parsing fails
        }
      } else {
        result = 'Ok'; // Default value if response is empty
      }

      console.log('Data posted:', result);
      setResponseData(result); // Lagre responsen i tilstanden

      // Tøm input-feltene etter vellykket innsending
      if (isString) {
        setData('');
      } else {
        setCreateUser({
          id: '',
          is_admin: false,
          email: '',
          first_name: '',
          last_name: '',
          budgetId: '',
        });
      }
    } catch (error) {
      console.error('Error posting data:', error);
      alert('Error posting data. Check console for details.');
    }
  };

  return (
    <div className="grid-container">
      <div className="grid-item">
        <h2>User getByMail</h2>
        <form onSubmit={(e) => handleSubmit(e, 'user/getall', "POST", getUser, setGetUser, true)}>

          <button type="submit">Send</button>
        </form>
      </div>
      <div className="grid-item">
        <h2>User getByMail</h2>
        <form onSubmit={(e) => handleSubmit(e, 'user/get', "POST", getUser, setGetUser, true)}>
          <input
            type="text"
            name="mail"
            placeholder="mail"
            value={getUser}
            onChange={(e) => {setGetUser(e.target.value);}}
          />
          <button type="submit">Send</button>
        </form>
      </div>
      <div className="grid-item">
        <h2>Create User</h2>
        <form onSubmit={(e) => handleSubmit(e, 'user/create', "POST", createUser, setCreateUser)}>
          <input
            type="text"
            name="email"
            placeholder="email"
            value={createUser.email}
            onChange={(e) => handleInputChange(e, setCreateUser)}
          />
          <input
            type="text"
            name="first_name"
            placeholder="first name"
            value={createUser.first_name}
            onChange={(e) => handleInputChange(e, setCreateUser)}
          />
          <input
            type="text"
            name="last_name"
            placeholder="last name"
            value={createUser.last_name}
            onChange={(e) => handleInputChange(e, setCreateUser)}
          />

          <button type="submit">Send</button>
        </form>
      </div>

      <div className="grid-item">
        <h2>Update User</h2>
        <form onSubmit={(e) => handleSubmit(e, 'user/update', "PUT", updateUser, setUpdateUser)}>
          <input
            type="text"
            name="id"
            placeholder="id"
            value={updateUser.id}
            onChange={(e) => handleInputChange(e, setUpdateUser)}
          />
          <input
            type="text"
            name="email"
            placeholder="email"
            value={updateUser.email}
            onChange={(e) => handleInputChange(e, setUpdateUser)}
          />
          <input
            type="text"
            name="first_name"
            placeholder="first name"
            value={updateUser.first_name}
            onChange={(e) => handleInputChange(e, setUpdateUser)}
          />
          <input
            type="text"
            name="last_name"
            placeholder="last name"
            value={updateUser.last_name}
            onChange={(e) => handleInputChange(e, setUpdateUser)}
          />

          <button type="submit">Send</button>
        </form>
      </div>

      <div className="grid-item">
        <h2>User Delete</h2>
        <form onSubmit={(e) => handleSubmit(e, 'user/delete', "DELETE", deleteUser, setGetUser, true)}>
          <input
            type="text"
            name="id"
            placeholder="id"
            value={deleteUser}
            onChange={(e) => {setDeleteUser(e.target.value)}}
          />
          <button type="submit">Send</button>
        </form>
      </div>

      {responseData && (
        <div className="response-container">
          <h3>Response Data:</h3>
          <pre>{JSON.stringify(responseData, null, 2)}</pre>
        </div>
      )}
    </div>
  );
};

export default App;
