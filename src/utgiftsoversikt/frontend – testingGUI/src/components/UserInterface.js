import React from 'react';

const UserInterface = ({
  title,
  getUser, setGetUser,
  createUser, setCreateUser,
  updateUser, setUpdateUser,
  deleteUser, setDeleteUser,
  handleSubmit, handleInputChange,
  responseData
}) => {
  return (
    <>
      <h2>{title}</h2>
      <div className="grid-container">
        <div className="grid-item">
          <h2>User getAll</h2>
          <form onSubmit={(e) => handleSubmit(e, 'user/getall', "POST", getUser, setGetUser, true)}>
            <button type="submit">Send</button>
          </form>
        </div>
        <div className="grid-item">
          <h2>User get</h2>
          <form onSubmit={(e) => handleSubmit(e, 'user/get', "POST", getUser, setGetUser, true)}>
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
          <h2>Delete User</h2>
          <form onSubmit={(e) => handleSubmit(e, 'user/delete', "DELETE", deleteUser, setGetUser, true)}>
            <input
              type="text"
              name="id"
              placeholder="id"
              value={deleteUser}
              onChange={(e) => { setDeleteUser(e.target.value); }}
            />
            <button type="submit">Send</button>
          </form>
        </div>


      </div>
    </>
  );
};

export default UserInterface;
