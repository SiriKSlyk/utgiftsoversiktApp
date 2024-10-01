import React, { useEffect, useState } from 'react';

import './OverviewInterface.css';
import Logout from './Logout';

const OverviewInterface = (
    setIsAuthenticated,
    isAuthenticated,
) => {

    const [expenses, setExpenses] = useState([]);
    const [budget, setBudget] = useState([]);
    const [month, setMonth] = useState([]);
    const [user, setUser] = useState([]);
    const [error, setError] = useState(null);

    useEffect(() => {
        console.log(isAuthenticated)
        const fetchExpenses = async () => {
            const token = localStorage.getItem('token');
            try {
                const response = await fetch('https://localhost:7062/auth/getall', { // Bytt ut med din API URL
                    method: 'POST',
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    },
                    credentials: 'include',
                    payload: {},
                });

                if (!response.ok) {
                    throw new Error('Failed to fetch expenses');
                }

                const data = await response.json();
                setExpenses(data.expenses); // Setter expenses i state
                setBudget(data.budget); // Setter expenses i state
                setMonth(data.month); // Setter expenses i state
                setUser(data.user)
                console.log(data)
            } catch (err) {
                setError(err.message);
                console.error('Error fetching expenses:', err);
            }
        };

        fetchExpenses();
        
    }, []); // Kjør kun ved første rendering

    return (
        <>
            {/* Navbar */}
            <div className="navbar">
                <h1>Oversikt</h1>
                <div className="profile">
                <Logout setIsAuthenticated={setIsAuthenticated} />
                    <span>{user.first_name} {user.last_name}</span>
                    <div className="avatar"></div>
                </div>
            </div>

            {/* Top Section */}
            <div className='top'>
                <div className='header-item'>
                    <h3 style={{ color: '#000000' }} className='spent'>{month.sum}kr</h3>
                    <h3>Brukt</h3>
                </div>
                <div className='header-item'>
                    <h3 style={{ color: '#000000' }} className='budget'>{budget.sum}kr</h3>
                    <h3>Budsjett</h3>
                </div>
                <div className='header-item'>
                    <h3 style={{ color: budget.sum - month.sum > 0 ? 'green' : budget.sum - month.sum < 0 ? 'red' : '#000000' }} className='remaining'>{budget.sum - month.sum}kr</h3>
                    <h3>Igjen</h3>
                </div>
            </div>

            {/* Title */}
            <h2 className="title">Oversikt</h2>

            {/* Category Section */}
            <div className='section'>
                <div className='category-card'>
                    <h3>Bolig</h3>
                    <p style={{ color: budget.house - month.house > 0 ? 'green' : budget.house - month.house < 0 ? 'red' : '#000000' }} className="spent">{budget.house - month.house}</p>
                    <p>Budsjett {budget.house} kr</p>
                    <p>Brukt {month.house} kr</p>
                </div>
                <div className='category-card'>
                    <h3>Mat</h3>
                    <p style={{ color: budget.food - month.food > 0 ? 'green' : budget.food - month.food < 0 ? 'red' : '#000000' }} className="spent">{budget.food - month.food}</p>
                    <p>Budsjett {budget.food} kr</p>
                    <p>Brukt {month.food} kr</p>
                </div>
                <div className='category-card'>
                    <h3>Transport</h3>
                    <p style={{ color: budget.transport - month.transport > 0 ? 'green' : budget.transport - month.transport < 0 ? 'red' : '#000000' }} className="spent">{budget.transport - month.transport}</p>
                    <p>Budsjett {budget.transport} kr</p>
                    <p>Brukt {month.transport} kr</p>
                </div>
                <div className='category-card'>
                    <h3>Gjeld</h3>
                    <p style={{ color: budget.debt - month.debt > 0 ? 'green' : budget.debt - month.debt < 0 ? 'red' : '#000000' }} className="spent">{budget.debt - month.debt}</p>
                    <p >Budsjett {budget.debt} kr</p>
                    <p>Brukt {month.debt} kr</p>
                </div>
                <div className='category-card'>
                    <h3>Sparing</h3>
                    <p style={{ color: budget.saving - month.saving > 0 ? 'green' : budget.saving - month.saving < 0 ? 'red' : '#000000' }} className="spent">{budget.saving - month.saving}</p>
                    <p>Budsjett {budget.saving} kr</p>
                    <p>Brukt {month.saving} kr</p>
                </div>
                <div className='category-card'>
                    <h3>Annet</h3>
                    <p style={{ color:budget.etc - month.etc > 0 ? 'green' : budget.etc - month.etc < 0 ? 'red' : '#000000' }} className="spent">{budget.etc - month.etc}</p>
                    <p>Budsjett {budget.etc} kr</p>
                    <p>Brukt {month.etc} kr</p>
                </div>
                
                {/* Add more category cards here */}
            </div>

            {/* Controls */}
            <div className="controls">
                <label>Vis:</label>
                <select>
                    <option>Alle</option>
                </select>

                <label>Filtrer etter:</label>
                <select>
                    <option>Alle</option>
                    {/*<option>Bolig</option>
                    <option>Mat</option>
                    <option>Transport</option>
                    <option>Gjeld</option>
                    <option>Sparing</option>
                    <option>Annet</option>
                    <option>Abonnomenter</option>*/}
                </select>

                {/*<button>+ Legg til fast utgift</button>
                <button>+ Legg til</button>*/}
            </div>

            {/* Table */}
            <div className='table'>
                <table>
                    <thead>
                        <tr>
                            <th>Dato</th>
                            <th>Kategori</th>
                            <th>Sum</th>
                            <th>Butikk</th>
                            <th>Beskrivelse</th>
                        </tr>
                    </thead>
                    <tbody>
                    
                    {expenses.map((expense, index) => (
                            <tr key={index}>
                                <td>{expense.date}</td>
                                <td>{expense.category}</td>
                                <td>{expense.sum.toFixed(2)} kr</td> {/* Formatere sum med to desimaler */}
                                <td>{expense.shop}</td>
                                <td>{expense.description}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>
        </>
    );
};

export default OverviewInterface;
