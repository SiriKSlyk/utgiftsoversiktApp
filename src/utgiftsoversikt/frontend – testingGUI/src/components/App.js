import React, { useState } from 'react';
import './App.css';
import UserInterface from './UserInterface'; // Importer komponenten
import MonthInterface from './MonthInterface'; // Importer komponenten
import ExpenseInterface from './ExpenseInterface'; // Importer komponenten
import BudgetInterface from './BudgetInterface'; // Importer komponenten
import LoginInterface from './LoginInterface'; // Importer komponenten

const App = () => {
    const [data, setData] = useState([]);
    const [getUser, setGetUser] = useState('');
    const [createUser, setCreateUser] = useState({ id: '', is_admin: false, email: '', first_name: '', last_name: '', budgetId: '' });
    const [updateUser, setUpdateUser] = useState({ id: '', is_admin: false, email: '', first_name: '', last_name: '', budgetId: '' });
    const [deleteUser, setDeleteUser] = useState('');
    const [responseData, setResponseData] = useState('');

    const [getMonth, setGetMonth] =useState({ id: '', userId: '', monthYear: '', budgetId: '', house: 0, food: 0, transport: 0, cloths: 0, debt: 0, saving: 0, etc: 0, sum: 0 });
    const [getAllMonth, setGetAllMonth] = useState('');
    const [createMonth, setCreateMonth] = useState({ id: '', userId: '', monthYear: '', budgetId: '', house: 0, food: 0, transport: 0, cloths: 0, debt: 0, saving: 0, etc: 0, sum: 0 });
    const [updateMonth, setUpdateMonth] = useState({ id: '', userId: '', monthYear: '', budgetId: '', house: 0, food: 0, transport: 0, cloths: 0, debt: 0, saving: 0, etc: 0, sum: 0 });
    const [deleteMonth, setDeleteMonth] = useState('');

    const [getExpense, setGetExpense] = useState('');
    const [getAllExpense, setGetAllExpense] = useState( {id: '', userId: '', month: '', date: '', shop: '', category: '', sum: 10.0, description:''});
    const [createExpense, setCreateExpense] = useState( {id: '', userId: '', month: '', date: '', shop: '', category: '', sum: 0, description:''});
    const [updateExpense, setUpdateExpense] = useState('');
    const [deleteExpense, setDeleteExpense] = useState('');

    const [getBudget, setGetBudget] = useState('');
    const [getAllBudget, setGetAllBudget] = useState('');
    const [createBudget, setCreateBudget] = useState( {id: '', userId: '', house: 0, food: 0, transport: 0, debt: 0, saving: 0, etc:0, sum: 0} );
    const [updateBudget, setUpdateBudget] = useState('');
    const [deleteBudget, setDeleteBudget] = useState('');

    const [loginData, setLoginData] = useState('');


    {/*const baseUrl = 'https://utgiftsoversikt.livelyriver-2c344957.northeurope.azurecontainerapps.io/';*/}
    const baseUrl = 'https://localhost:7062/';

    const handleInputChange = (e, setPostData) => {
        const { name, value, type, checked } = e.target;
        setPostData(prevData => ({
            ...prevData,
            [name]: type === 'checkbox' ? checked : value
        }));
    };

    const loginSubmit = async (e, endpoint, method, postData, setData, isString = false) => {
        e.preventDefault();
        const url = `${baseUrl}${endpoint}`;
        const payload = JSON.stringify(postData);
        console.log(payload)
        try {
            const response = await fetch(url, {
                method: method,
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
            localStorage.setItem('token', token);
            console.log(localStorage.getItem('token'))

            

            //setResponseData(data ? JSON.parse(data) : 'Ok');
            //setData(isString ? '' : data);
            console.log(data);

        } catch (error) {
            console.error('Error:', error);
        }
    };

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
            setResponseData(result ? JSON.parse(result) : 'Ok');
            setData(isString ? '' : { id: '', is_admin: false, email: '', first_name: '', last_name: '', budgetId: '' });

        } catch (error) {
            console.error('Error:', error);
        }
    };

    return (
        <>
        {responseData && (
                <div className="response-container">
                    <h3>Response Data:</h3>
                    <pre>{JSON.stringify(responseData, null, 2)}</pre>
                </div>
            )}
            <LoginInterface
                title={"Login"}
                loginData={loginData}
                setLoginData={setLoginData}
                handleSubmit={loginSubmit}
                handleInputChange={handleInputChange}
            />

            

            <UserInterface
                title={"User"}
                getUser={getUser}
                setGetUser={setGetUser}
                createUser={createUser}
                setCreateUser={setCreateUser}
                updateUser={updateUser}
                setUpdateUser={setUpdateUser}
                deleteUser={deleteUser}
                setDeleteUser={setDeleteUser}
                handleSubmit={handleSubmit}
                handleInputChange={handleInputChange}
            />

            <MonthInterface
                title={"Month"}
                getMonth={getMonth}
                setGetMonth={setGetMonth}
                getAllMonth={getAllMonth}
                setGetAllMonth={setGetAllMonth}
                createMonth={createMonth}
                setCreateMonth={setCreateMonth}
                updateMonth={updateMonth}
                setUpdateMonth={setUpdateMonth}
                deleteMonth={deleteMonth}
                setDeleteMonth={setDeleteMonth}
                handleSubmit={handleSubmit}
                handleInputChange={handleInputChange}
            />

            <ExpenseInterface
                title={"Expense"}
                getExpense={getExpense}
                setGetExpense={setGetExpense}
                getAllExpense={getAllExpense}
                setGetAllExpense={setGetAllExpense}
                createExpense={createExpense}
                setCreateExpense={setCreateExpense}
                updateExpense={updateExpense}
                setUpdateExpense={setUpdateExpense}
                deleteExpense={deleteExpense}
                setDeleteExpense={setDeleteExpense}
                handleSubmit={handleSubmit}
                handleInputChange={handleInputChange}
            />

            <BudgetInterface
                title={"Budget"}
                getBudget={getBudget}
                setGetBudget={setGetBudget}
                getAllBudget={getAllBudget}
                setGetAllBudget={setGetAllBudget}
                createBudget={createBudget}
                setCreateBudget={setCreateBudget}
                updateBudget={updateBudget}
                setUpdateBudget={setUpdateBudget}
                deleteBudget={deleteBudget}
                setDeleteBudget={setDeleteBudget}
                handleSubmit={handleSubmit}
                handleInputChange={handleInputChange}
            />

            
        </>

    );
};

export default App;
