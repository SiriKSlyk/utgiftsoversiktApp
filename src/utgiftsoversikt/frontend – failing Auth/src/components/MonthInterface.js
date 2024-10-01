import React from 'react';

const MonthInterface = ({
  title,
  getMonth, setGetMonth,
  getAllMonth, setGetAllMonth,
  createMonth, setCreateMonth,
  updateMonth, setUpdateMonth,
  deleteMonth, setDeleteMonth,
  handleSubmit, handleInputChange,
}) => {
  return (
    <>
      <h2>{title}</h2>
      <div className="grid-container">

        <div className="grid-item">
          <h2>Month all</h2>
          <form onSubmit={(e) => handleSubmit(e, 'month/getall', "POST", getAllMonth, setGetAllMonth, true)}>
            <input
              type="text"
              name="userId"
              placeholder="User id"
              value={getAllMonth.userId}
              onChange={(e) => { setGetAllMonth(e.target.value); }}
            />
            <button type="submit">Send</button>
          </form>
        </div>
        <div className="grid-item">
          <h2>Month getByIdAndMonth</h2>
          <form onSubmit={(e) => handleSubmit(e, 'month/get', "POST", getMonth, setGetMonth, true)}>
            <input
              type="text"
              name="userId"
              placeholder="User id"
              value={getMonth.userId}
              onChange={(e) => handleInputChange(e, setGetMonth)}
            />
            <input
              type="text"
              name="monthYear"
              placeholder="MMYYYY"
              value={getMonth.monthYear}
              onChange={(e) => handleInputChange(e, setGetMonth)}
            />
            <button type="submit">Send</button>
          </form>
        </div>
        <div className="grid-item">
          <h2>Create month</h2>
          <form onSubmit={(e) => handleSubmit(e, 'month/create', "POST", createMonth, setCreateMonth)}>
            <input
              type="text"
              name="userId"
              placeholder="userId"
              value={createMonth.userId}
              onChange={(e) => handleInputChange(e, setCreateMonth)}
            />
            <input
              type="text"
              name="monthYear"
              placeholder="MMYYYY"
              value={createMonth.monthYear}
              onChange={(e) => handleInputChange(e, setCreateMonth)}
            />
            <input
              type="text"
              name="budgetId"
              placeholder="BudgetId"
              value={createMonth.budgetId}
              onChange={(e) => handleInputChange(e, setCreateMonth)}
            />
            <button type="submit">Send</button>
          </form>
        </div>

        <div className="grid-item">
          <h2>Update month</h2>
          <form onSubmit={(e) => handleSubmit(e, 'month/update', "PUT", updateMonth, setUpdateMonth)}>
            <input
              type="text"
              name="id"
              placeholder="id"
              value={updateMonth.id}
              onChange={(e) => handleInputChange(e, setUpdateMonth)}
            />
            <input
              type="text"
              name="userId"
              placeholder="userId"
              value={updateMonth.userId}
              onChange={(e) => handleInputChange(e, setUpdateMonth)}
            />
            <input
              type="text"
              name="monthYear"
              placeholder="MMYYYY"
              value={updateMonth.monthYear}
              onChange={(e) => handleInputChange(e, setUpdateMonth)}
            />
            <input
              type="text"
              name="budgetId"
              placeholder="BudgetId"
              value={updateMonth.budgetId}
              onChange={(e) => handleInputChange(e, setUpdateMonth)}
            />
            <button type="submit">Send</button>
          </form>
        </div>

        <div className="grid-item">
          <h2>Delete month</h2>
          <form onSubmit={(e) => handleSubmit(e, 'month/delete', "DELETE", deleteMonth, setGetMonth, true)}>
            <input
              type="text"
              name="id"
              placeholder="id"
              value={deleteMonth}
              onChange={(e) => { setDeleteMonth(e.target.value); }}
            />
            <button type="submit">Send</button>
          </form>
        </div>


      </div>
    </>
  );
};

export default MonthInterface;
