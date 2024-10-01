import { useRef, useState, useEffect } from 'react';
import useAuth from '../hooks/userAuth';
import { Link, useNavigate, useLocation } from 'react-router-dom'

import axios from '../api/axios';
const LOGIN_URL = '/auth/login';

const Login = () => {
    const { setAuth } = useAuth();

    const navigate = useNavigate();
    const location = useLocation();
    const from = location.state?.from?.pathname || "/";

    const userRef = useRef();
    const errRef = useRef();

    const [user, setUser] = useState('')
    const [errMsg, setErrMsg] = useState('')

    useEffect(() => {
        userRef.current.focus();
    }, [])

    useEffect(() => {
        setErrMsg('');
    }, [user])

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await axios.post(LOGIN_URL,
                user,
                {
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    credentials: 'include',
                    body: { email: user }
                }

            );
            console.log(JSON.stringify * (response))
            const token = response?.token;
            setAuth({ user, token })
            setUser('');
            navigate(from, { replace: true });
        } catch (err) {
            if (!err.response) {
                setErrMsg('No server response')
            } else if (err.response?.status === 400) {
                errMsg('Missing username')
            } else if (err.response?.status === 401) {
                errMsg('Unautherized')
            } else {
                setErrMsg('Login failed')
            }
            errRef.current.focus();
        }
    }

    return (
        <section>
            <p ref={errRef} className={errMsg ? "errmsg" : "offscreen"} aria-live="assertive">{errMsg}</p>
            <h1>Sign in</h1>
            <form onSubmit={handleSubmit}>
                <label htmlFor="username">Username:</label>
                <input
                    type="text"
                    id="username"
                    ref={userRef}
                    onChange={(e) => setUser(e.target.value)}
                    value={user}
                    required
                />
                <button>Sign in</button>
            </form>
        </section>

    );
};

export default Login;
