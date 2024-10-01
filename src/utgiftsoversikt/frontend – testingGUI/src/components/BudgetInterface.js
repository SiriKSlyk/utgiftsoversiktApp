import React from 'react';

const BudgetInterface = ({
  title,
  getBudget, setGetBudget,
  getAllBudget, setGetAllBudget,
  createBudget, setCreateBudget,
  updateBudget, setUpdateBudget,
  deleteBudget, setDeleteBudget,
  handleSubmit, handleInputChange,
}) => {
  return (
    <>
      <h2>{title}</h2>
      <div className="grid-container">

        <div className="grid-item">
          <h2>Budget All</h2>
          <form onSubmit={(e) => handleSubmit(e, 'budget/getall', "POST", getAllBudget, setGetAllBudget, true)}>
            <input
              type="text"
              name="userId"
              placeholder="User id"
              value={getAllBudget.userId}
              onChange={(e) => { setGetAllBudget(e.target.value); }}
            />


            <button type="submit">Send</button>
          </form>
        </div>
        <div className="grid-item">
          <h2>Budget getById</h2>
          <form onSubmit={(e) => handleSubmit(e, 'budget/get', "POST", getBudget, setGetBudget, true)}>
            Budget id
            <input
              type="text"
              name="id"
              placeholder="Budget id"
              value={getBudget.id}
              onChange={(e) => { setGetBudget(e.target.value); }}
            />
            <button type="submit">Send</button>
          </form>
        </div>
        <div className="grid-item">
          <h2>Create Budget</h2>
          <form onSubmit={(e) => handleSubmit(e, 'budget/create', "POST", createBudget, setCreateBudget)}>
            <p>User id:</p>
            <input
              type="text"
              name="userId"
              placeholder="userId"
              value={createBudget.userId}
              onChange={(e) => handleInputChange(e, setCreateBudget)}
            />
            <p>House budget:</p>
            <input
              type="number"
              name="house"
              placeholder="Budget house"
              value={createBudget.date}
              onChange={(e) => handleInputChange(e, setCreateBudget)}
            />
            <p>Food budget:</p>
            <input
              type="number"
              name="food"
              placeholder="Budget food"
              value={createBudget.shop}
              onChange={(e) => handleInputChange(e, setCreateBudget)}
            />
            <p>Transport budget:</p>
            <input
              type="number"
              name="transport"
              placeholder="Budget transport"
              value={createBudget.transport}
              onChange={(e) => handleInputChange(e, setCreateBudget)}
            />
            <p>Dept budget:</p>
            <input
              type="number"
              name="debt"
              placeholder="Budget debt"
              value={createBudget.debt}
              onChange={(e) => handleInputChange(e, setCreateBudget)}
            />
            <p>Savings budget:</p>
            <input
              type="number"
              name="saving"
              placeholder="Budget saving"
              value={createBudget.saving}
              onChange={(e) => handleInputChange(e, setCreateBudget)}
            />
            <p>Etc budget:</p>
            <input
              type="number"
              name="etc"
              placeholder="Budget etc"
              value={createBudget.etc}
              onChange={(e) => handleInputChange(e, setCreateBudget)}
            />
            <p>Total budget:</p>
            <input
              type="number"
              name="sum"
              placeholder="sum"
              value={createBudget.sum}
              onChange={(e) => handleInputChange(e, setCreateBudget)}
            />

            <button type="submit">Send</button>
          </form>
        </div>

        <div className="grid-item">
          <h2>Update Budget</h2>
          <form onSubmit={(e) => handleSubmit(e, 'budget/update', "PUT", updateBudget, setUpdateBudget)}>
          <p>Budget id:</p>
            <input
              type="text"
              name="id"
              placeholder="id"
              value={setUpdateBudget.id}
              onChange={(e) => handleInputChange(e, setUpdateBudget)}
            />
            <p>User id:</p>
            <input
              type="text"
              name="userId"
              placeholder="userId"
              value={setUpdateBudget.userId}
              onChange={(e) => handleInputChange(e, setUpdateBudget)}
            />
            <p>House budget:</p>
            <input
              type="number"
              name="house"
              placeholder="Budget house"
              value={setUpdateBudget.date}
              onChange={(e) => handleInputChange(e, setUpdateBudget)}
            />
            <p>Food budget:</p>
            <input
              type="number"
              name="food"
              placeholder="Budget food"
              value={setUpdateBudget.shop}
              onChange={(e) => handleInputChange(e, setUpdateBudget)}
            />
            <p>Transport budget:</p>
            <input
              type="number"
              name="transport"
              placeholder="Budget transport"
              value={setUpdateBudget.transport}
              onChange={(e) => handleInputChange(e, setUpdateBudget)}
            />
            <p>Dept budget:</p>
            <input
              type="number"
              name="debt"
              placeholder="Budget debt"
              value={setUpdateBudget.debt}
              onChange={(e) => handleInputChange(e, setUpdateBudget)}
            />
            <p>Savings budget:</p>
            <input
              type="number"
              name="saving"
              placeholder="Budget saving"
              value={setUpdateBudget.saving}
              onChange={(e) => handleInputChange(e, setUpdateBudget)}
            />
            <p>Etc budget:</p>
            <input
              type="number"
              name="etc"
              placeholder="Budget etc"
              value={setUpdateBudget.etc}
              onChange={(e) => handleInputChange(e, setUpdateBudget)}
            />
            <p>Total budget:</p>
            <input
              type="number"
              name="sum"
              placeholder="sum"
              value={setUpdateBudget.sum}
              onChange={(e) => handleInputChange(e, setUpdateBudget)}
            />
            <button type="submit">Send</button>
          </form>
        </div>

        <div className="grid-item">
          <h2>Delete Budget</h2>
          <form onSubmit={(e) => handleSubmit(e, 'budget/delete', "DELETE", deleteBudget, setGetBudget, true)}>
            <input
              type="text"
              name="id"
              placeholder="id"
              value={deleteBudget}
              onChange={(e) => { setDeleteBudget(e.target.value); }}
            />
            <button type="submit">Send</button>
          </form>
        </div>


      </div>
    </>
  );
};

export default BudgetInterface;
