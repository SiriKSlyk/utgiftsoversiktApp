import React from 'react';

const ExpenseInterface = ({
  title,
  getExpense, setGetExpense,
  getAllExpense, setGetAllExpense,
  createExpense, setCreateExpense,
  updateExpense, setUpdateExpense,
  deleteExpense, setDeleteExpense,
  handleSubmit, handleInputChange,
}) => {
  return (
    <>
      <h2>{title}</h2>
      <div className="grid-container">

        <div className="grid-item">
          <h2>Expense All</h2>
          <form onSubmit={(e) => handleSubmit(e, 'expense/getall', "POST", getAllExpense, setGetAllExpense, true)}>
            <input
              type="text"
              name="userId"
              placeholder="User id"
              value={getAllExpense.userId}
              onChange={(e) => handleInputChange(e, setGetAllExpense)}
            />

            <input
              type="text"
              name="month"
              placeholder="MMYYYY"
              value={getAllExpense.month}
              onChange={(e) => handleInputChange(e, setGetAllExpense)}
            />

            <button type="submit">Send</button>
          </form>
        </div>
        <div className="grid-item">
          <h2>Expense getById</h2>
          <form onSubmit={(e) => handleSubmit(e, 'expense/get', "POST", getExpense, setGetExpense, true)}>
            <input
              type="text"
              name="id"
              placeholder="Id"
              value={getExpense.id}
              onChange={(e) => { setGetExpense(e.target.value); }}
            />
            <button type="submit">Send</button>
          </form>
        </div>
        <div className="grid-item">
          <h2>Create Expense</h2>
          <form onSubmit={(e) => handleSubmit(e, 'expense/create', "POST", createExpense, setCreateExpense)}>
            <input
              type="text"
              name="userId"
              placeholder="userId"
              value={createExpense.userId}
              onChange={(e) => handleInputChange(e, setCreateExpense)}
            />
            <input
              type="text"
              name="date"
              placeholder="DD-MM-YYYY"
              value={createExpense.date}
              onChange={(e) => handleInputChange(e, setCreateExpense)}
            />
            <input
              type="text"
              name="shop"
              placeholder="shop"
              value={createExpense.shop}
              onChange={(e) => handleInputChange(e, setCreateExpense)}
            />
            <input
              type="text"
              name="category"
              placeholder="category"
              value={createExpense.category}
              onChange={(e) => handleInputChange(e, setCreateExpense)}
            />
            <input
              type="text"
              name="sum"
              placeholder="sum"
              value={createExpense.sum}
              onChange={(e) => handleInputChange(e, setCreateExpense)}
            />
            <input
              type="text"
              name="description"
              placeholder="Description if needed"
              value={createExpense.description}
              onChange={(e) => handleInputChange(e, setCreateExpense)}
            />
            <button type="submit">Send</button>
          </form>
        </div>

        <div className="grid-item">
          <h2>Update Expense</h2>
          <form onSubmit={(e) => handleSubmit(e, 'expense/update', "PUT", updateExpense, setUpdateExpense)}>
            <input
              type="text"
              name="id"
              placeholder="id"
              value={updateExpense.id}
              onChange={(e) => handleInputChange(e, setUpdateExpense)}
            />
            <input
              type="text"
              name="userId"
              placeholder="userId"
              value={updateExpense.userId}
              onChange={(e) => handleInputChange(e, setUpdateExpense)}
            />
            <input
              type="text"
              name="date"
              placeholder="dd-MM-yyyy"
              value={updateExpense.date}
              onChange={(e) => handleInputChange(e, setUpdateExpense)}
            />
            <input
              type="text"
              name="shop"
              placeholder="shop"
              value={updateExpense.shop}
              onChange={(e) => handleInputChange(e, setUpdateExpense)}
            />
            <input
              type="text"
              name="category"
              placeholder="category"
              value={updateExpense.category}
              onChange={(e) => handleInputChange(e, setUpdateExpense)}
            />
            <input
              type="text"
              name="sum"
              placeholder="sum"
              value={updateExpense.sum}
              onChange={(e) => handleInputChange(e, setUpdateExpense)}
            />
            <input
              type="text"
              name="description"
              placeholder="Description if needed"
              value={updateExpense.description}
              onChange={(e) => handleInputChange(e, setUpdateExpense)}
            />
            <button type="submit">Send</button>
          </form>
        </div>

        <div className="grid-item">
          <h2>Delete Expense</h2>
          <form onSubmit={(e) => handleSubmit(e, 'expense/delete', "DELETE", deleteExpense, setGetExpense, true)}>
            <input
              type="text"
              name="id"
              placeholder="id"
              value={deleteExpense}
              onChange={(e) => { setDeleteExpense(e.target.value); }}
            />
            <button type="submit">Send</button>
          </form>
        </div>


      </div>
    </>
  );
};

export default ExpenseInterface;
