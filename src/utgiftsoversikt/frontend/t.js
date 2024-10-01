import React, { useState } from 'react';
import './App.css'; // Legg til en CSS-fil for styling

const App = () => {
  const [data, setData] = useState([]);
  const [postData2, setPostData2] = useState('');
  const [postData3, setPostData3] = useState({ id: '', is_admin: false, email: '', first_name: '', last_name: '', budgetId: '' });
  const baseUrl = 'https://utgiftsoversikt.bluedune-ec83153a.northeurope.azurecontainerapps.io/';

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
    const { name, value } = e.target;
    setPostData(prevData => ({ ...prevData, [name]: value }));
  };

  const handleStringInputChange = (e) => {
    setPostData2(e.target.value);
  };

  const handleSubmit = async (e, endpoint, postData, isString = false) => {
    e.preventDefault();
    const url = `${baseUrl}${endpoint}`;
    const payload = isString ? postData : JSON.stringify(postData);
    console.log(`Posting data to: ${url}`, payload); // Log payload to verify it's correct
    try {
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'accept': 'text/plain',
          'Content-Type': 'application/json',
        },
        body: isString ? JSON.stringify(postData) : payload,
      });
      console.log('Request payload:', payload); // Log the payload being sent
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      const result = await response.json();
      console.log('Data posted:', result);
      // Sjekk om resultatet er et array eller et enkeltobjekt
      setData(Array.isArray(result) ? result : [result]);
    } catch (error) {
      console.error('Error posting data:', error);
      alert('Error posting data. Check console for details.');
    }
  };

  return (
    <div>
      <h1>Data fra API</h1>
      <button onClick={fetchData}>Hent Data</button>
      <div className="grid-container">
        <div className="grid-item">
          <h2>User getAll</h2>
          <form onSubmit={(e) => handleSubmit(e, 'user/getall', {})}>
            <button type="submit">Send</button>
          </form>
        </div>
        <div className="grid-item">
          <h2>User getByMail</h2>
          <form onSubmit={(e) => handleSubmit(e, 'user/get', postData2, true)}>
            <input
              type="text"
              name="mail"
              placeholder="mail"
              value={postData2}
              onChange={handleStringInputChange}
            />
            <button type="submit">Send</button>
          </form>
        </div>
        <div className="grid-item">
          <h2>Create User</h2>
          <form onSubmit={(e) => handleSubmit(e, 'user/create', postData3)}>
            <input
              type="text"
              name="id"
              placeholder="id"
              value={postData3.id}
              onChange={(e) => handleInputChange(e, setPostData3)}
            />
            <input
              type="checkbox"
              name="is_admin"
              checked={postData3.is_admin}
              onChange={(e) => handleInputChange(e, setPostData3)}
            />
            <label htmlFor="is_admin">Is Admin</label>
            <input
              type="text"
              name="email"
              placeholder="email"
              value={postData3.email}
              onChange={(e) => handleInputChange(e, setPostData3)}
            />
            <input
              type="text"
              name="first_name"
              placeholder="first name"
              value={postData3.first_name}
              onChange={(e) => handleInputChange(e, setPostData3)}
            />
            <input
              type="text"
              name="last_name"
              placeholder="last name"
              value={postData3.last_name}
              onChange={(e) => handleInputChange(e, setPostData3)}
            />
            <input
              type="text"
              name="budgetId"
              placeholder="budgetId"
              value={postData3.budgetId}
              onChange={(e) => handleInputChange(e, setPostData3)}
            />
            <button type="submit">Send</button>
          </form>
        </div>
        <div className="grid-item">
          <h2>Update User</h2>
          <form onSubmit={(e) => handleSubmit(e, 'user/update', postData3)}>
            <input
              type="text"
              name="username"
              placeholder="username"
              value={postData3.username}
              onChange={(e) => handleInputChange(e, setPostData3)}
            />
            <input
              type="text"
              name="email"
              placeholder="email"
              value={postData3.email}
              onChange={(e) => handleInputChange(e, setPostData3)}
            />
            <button type="submit">Send</button>
          </form>
        </div>
        <div className="grid-item">
          <h2>Delete User</h2>
          <form onSubmit={(e) => handleSubmit(e, 'user/delete', postData2)}>
            <input
              type="text"
              name="userId"
              placeholder="userId"
              value={postData2.userId}
              onChange={(e) => handleInputChange(e, setPostData2)}
            />
            <button type="submit">Send</button>
          </form>
        </div>
      </div>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Navn</th>
            <th>Verdi</th>
          </tr>
        </thead>
        <tbody>
          {data.length > 0 ? (
            data.map(item => (
              <tr key={item.id}>
                <td>{item.id}</td>
                <td>{item.first_name}</td>
                <td>{item.email}</td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="3">Ingen data tilgjengelig</td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
};

export default App;
