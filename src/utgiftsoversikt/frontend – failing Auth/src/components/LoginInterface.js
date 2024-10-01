import React from 'react';

const LoginInterface = ({
  title,
  loginData, setLoginData,
  handleSubmit, handleInputChange,
  responseData
}

) => {
  return (
    
    <>
      <h2>{title}</h2>
      <div className="grid-container">
      <div className="grid-item">
          <h2>Login</h2>
          <form onSubmit={(e) => handleSubmit(e, 'auth/login', "POST", loginData, setLoginData, true)}>
            <input
              type="text"
              name="mail"
              placeholder="mail"
              value={loginData.mail}
              onChange={(e) => { setLoginData(e.target.value); }}
            />
            <button type="submit">Send</button>
          </form>
        </div>
        <div className="grid-item">
          <h2>Logout</h2>
          <form onSubmit={(e) => handleSubmit(e, 'auth/logout', "POST", loginData, setLoginData, true)}>
            <button type="submit">Send</button>
          </form>
        </div>
      </div>
    </>
  );
};

export default LoginInterface;
