import { useState, type ChangeEvent } from "react";
import { Form, useNavigate } from "react-router";
import { type LoginJSON, type IUserProfile, type RegisterJSON } from "../models/ViewModels";
import { APICall, ResponseCode, ResponseError } from "../services/APIService";
import { AttemptAuth } from "../services/AuthService";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";

export interface LoginProps {
    RegisterMode?: boolean
}

function AuthFormComponent({ RegisterMode = false }: LoginProps) {
    const [errorState, setErrorState] = useState('');

    const [userForm, setUserForm] = useState<RegisterJSON>({ Birthday: new Date()} as RegisterJSON)

    const navigate = useNavigate();

    const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;

        setUserForm(prev => ({...prev, [name]: value}));
    }

    const formValid = (): boolean => {
        if (!(userForm.Email.length > 10)) {
            setErrorState('Invalid email address.')
            return false;
        }

        if (!(userForm.Password.length > 8)) {
            setErrorState('Invalid password.')
            return false;
        }


        if (RegisterMode && userForm.Password != userForm.ConfirmPassword) {
            setErrorState('Passwords do not match.')
            return false;
        }

        return true;
    }

    const login = async () => {
        let res = await AttemptAuth(userForm);
        if (res.success) {
            navigate("/");
            return;
        }

        let message = "Login failed. ";

        // TODO SWITCH
        if (res.error.status == ResponseCode.Unauthorized) {
            message += "Incorrect username or password."
        } else {
            message += "Unspecified error occurred (" + ResponseCode[res.error.status] + ")";
        }

        setErrorState(message);
    };

    const register = async () => {
        let res = await AttemptAuth(userForm, true);

        if (!res.success)
        {
            setErrorState(res.error.message ?? "An unspecified error has occurred. Please contact us for assistance.");
        }

        navigate("/");
    };

    const setBirthday = (date: Date | null) => {
        setUserForm(prev => ({...prev, ["Birthday"]: date ?? new Date()}));
    };

    return (
        <div className="popup-wrapper bg-white d-flex justify-content-center align-items-center shadow-lg rounded-5 p-5">
                <form>
                    <div className="container">
                        {RegisterMode ? (
                            <>
                                <div className="row">
                                    <div className="col d-flex justify-content-center mt-3 mb-0">
                                        <h1>Register your Account</h1>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col d-flex justify-content-center m-3 mb-5 text-underline">
                                        <a href="/login">Already have an account?</a>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col-12 col-md-6">
                                        <div className="input-group input-group-lg mb-3">
                                            <span className="input-group-text" id="inputGroup-sizing-lg">First Name</span>
                                            <input type="text" name="FirstName" className="form-control" aria-label="Registrant First Name" aria-describedby="inputGroup-sizing-lg" onChange={handleChange}  value={userForm.FirstName}/>
                                        </div>
                                    </div>

                                    <div className="col-12 col-md-6">
                                        <div className="input-group input-group-lg mb-3">
                                            <span className="input-group-text" id="inputGroup-sizing-default">Last Name</span>
                                            <input type="text" name="LastName" className="form-control" aria-label="Registrant Last Name" aria-describedby="inputGroup-sizing-sefault" onChange={handleChange}  value={userForm.LastName}/>
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col m-0">

                                        <div className="input-group mb-3">
                                            <span className="input-group-text" id="inputGroup-sizing-default">Birthday</span>
                                            
                                            <div className="flex-grow-1">
                                                <DatePicker
                                                    className="form-control rounded-0 rounded-end w-100"
                                                    selected={userForm.Birthday}
                                                    onChange={(date) => {setBirthday(date);}}
                                                    dateFormat="yyyy-MM-dd"
                                                    isClearable
                                                    placeholderText="Select your birthday..."
                                                    aria-describedby="inputGroup-sizing-default"
                                                />
                                            </div>
                                        </div>

                                        

                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <div className="input-group input-group-lg mb-3">
                                            <span className="input-group-text" id="inputGroup-sizing-default">Email</span>
                                            <input type="email" name="Email" className="form-control" aria-label="Registrant Email Address" aria-describedby="inputGroup-sizing-default" onChange={handleChange}  value={userForm.Email}/>
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <div className="input-group input-group-lg mb-3">
                                            <span className="input-group-text" id="inputGroup-sizing-default">Phone Number</span>
                                            <input type="tel" name="PhoneNumber" className="form-control" aria-label="Registrant Phone Number" aria-describedby="inputGroup-sizing-default" onChange={handleChange}  value={userForm.PhoneNumber}/>
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <div className="input-group input-group-lg mb-3">
                                            <span className="input-group-text" id="inputGroup-sizing-default">Password</span>
                                            <input type="password" name="Password" className="form-control" aria-label="Registrant Password" aria-describedby="inputGroup-sizing-default" onChange={handleChange}  value={userForm.Password}/>
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <div className="input-group input-group-lg mb-3">
                                            <span className="input-group-text" id="inputGroup-sizing-default">Confirm Password</span>
                                            <input type="password" name="ConfirmPassword" className="form-control" aria-label="Confirm Registrant Password" aria-describedby="inputGroup-sizing-default" onChange={handleChange}  value={userForm.ConfirmPassword}/>
                                        </div>
                                    </div>
                                </div>

                                <div className="row">
                                    <div className="col">
                                        <button onClick={e => { e.preventDefault(); register(); } } className="btn btn-lg btn-success w-100">Register</button>
                                    </div>
                                </div>
                            </>
                        ) : 
                        (<>
                            <div className="row">
                                <div className="col">
                                    <div className="input-group input-group-lg mb-3">
                                        <span className="input-group-text" id="inputGroup-sizing-default">Email</span>
                                        <input type="email" name="Email" className="form-control" aria-label="Registrant Email Address" aria-describedby="inputGroup-sizing-default" onChange={handleChange}  value={userForm.Email}/>
                                    </div>
                                </div>
                            </div>

                            <div className="row">
                                <div className="col">
                                    <div className="input-group input-group-lg mb-3">
                                        <span className="input-group-text" id="inputGroup-sizing-default">Password</span>
                                        <input type="password" name="Password" className="form-control" aria-label="Registrant Password" aria-describedby="inputGroup-sizing-default" onChange={handleChange}  value={userForm.Password}/>
                                    </div>
                                </div>
                            </div>

                            <div className="row">
                                <div className="col">
                                    <button onClick={e => { e.preventDefault(); login(); } } className="btn btn-lg btn-success">Login</button>
                                </div>
                            </div>

                            
                        </>)
                        }
                        

                        <p className="danger">{errorState}</p>


                    </div>
                    
                </form>
        </div>
  );
}



export default AuthFormComponent;