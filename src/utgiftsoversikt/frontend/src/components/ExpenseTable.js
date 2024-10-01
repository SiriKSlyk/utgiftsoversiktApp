import React, { useState } from 'react';


const ExpenseTable = () => {
    const [expenses, setExpenses] = useState([
        { date: '09.09.2024', category: 'Food', sum: 100.0, shop: 'Kiwi', description: 'Description' }
    ]);

    // Funksjon for å legge til en ny tom rad
    const addEmptyExpense = () => {
        setExpenses([...expenses, { date: '', category: '', sum: '', shop: '', description: '' }]);
    };

    // Funksjon for å håndtere oppdatering av verdier i en rad
    const handleInputChange = (index, event) => {
        const { name, value } = event.target;
        const updatedExpenses = [...expenses];
        updatedExpenses[index][name] = value;
        setExpenses(updatedExpenses);
    };

    // Funksjon for å sende data til backend (kan oppdateres med riktig API-kall)
    const handleSubmit = (index) => {
        const expense = expenses[index];
        console.log('Submitting expense:', expense);
        // Her kan du sende expense-data til backend (API-anrop)
    };

    return (
        <table>
            <thead>
                <tr>
                    <th>Dato</th>
                    <th>Kategori</th>
                    <th>Sum</th>
                    <th>Butikk</th>
                    <th>Beskrivelse</th>
                    <th>Submit</th>
                </tr>
            </thead>
            <tbody>
                {expenses.map((expense, index) => (
                    <tr key={index}>
                        <td>
                            <input
                                type="date"
                                name="date"
                                value={expense.date}
                                onChange={(e) => handleInputChange(index, e)}
                            />
                        </td>
                        <td>
                            <input
                                type="text"
                                name="category"
                                value={expense.category}
                                onChange={(e) => handleInputChange(index, e)}
                            />
                        </td>
                        <td>
                            <input
                                type="number"
                                name="sum"
                                value={expense.sum}
                                onChange={(e) => handleInputChange(index, e)}
                            />
                        </td>
                        <td>
                            <input
                                type="text"
                                name="shop"
                                value={expense.shop}
                                onChange={(e) => handleInputChange(index, e)}
                            />
                        </td>
                        <td>
                            <input
                                type="text"
                                name="description"
                                value={expense.description}
                                onChange={(e) => handleInputChange(index, e)}
                            />
                        </td>
                        <td>
                            <button onClick={() => handleSubmit(index)}>Submit</button>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
};

export default ExpenseTable;
