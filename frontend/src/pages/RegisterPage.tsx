import { Page } from "./Page";
import React = require("react");
import { TextInput } from "../components/TextInput";
import { User } from "../backend/User";
import { Application } from "../main";
import { validateEmail } from "../utility/Validators";

type RegistrationType = "employee" | "company";

interface RegisterPageState {
    type: RegistrationType;
    name: string;
    lastName: string;
    location: string;
    email: string;
    password: string;
    confirmPassword: string;
    error: string;
}

export class RegisterPage extends Page<RegisterPageState> {
    state = {
        type: "employee" as RegistrationType,
        name: "",
        lastName: "",
        location: "",
        email: "",
        password: "",
        confirmPassword: "",
        error: ""
    };

    constructor(props: {}) {
        super(props);
    }

    render() {
        const $ = this.binder;
        const emailValidator = (message: string) => {
            return (email: string) => {
                if (!validateEmail(email)) {
                    return message;
                }

                return true;
            };
        }

        return <div className="register-page">
            <div className="register-form">
                <div className="type">
                    <div onClick={() => this.selectType("employee")} className={this.state.type === "employee" ? "selected" : ""}>Employee</div>
                    <div onClick={() => this.selectType("company")} className={this.state.type === "company" ? "selected" : ""}>Company</div>
                </div>
                {this.state.type === "employee" ? <div className="data">
                    <TextInput placeholder="name" value={$("name")}/>
                    <TextInput placeholder="last name" value={$("lastName")}/>
                    <TextInput placeholder="email" value={$("email")} validator={emailValidator("Specified email is invalid")}/>
                    <TextInput password={true} placeholder="password" value={$("password")}/>
                    <TextInput password={true} placeholder="confirm password" value={$("confirmPassword")}/>
                </div> : <div className="data">
                    <TextInput placeholder="company name" value={$("name")}/>
                    <TextInput placeholder="company address" value={$("location")}/>
                    <TextInput placeholder="email" value={$("email")} validator={emailValidator("Specified email is invalid")}/>
                    <TextInput password={true} placeholder="password" value={$("password")}/>
                    <TextInput password={true} placeholder="confirm password" value={$("confirmPassword")}/>
                </div>}
                <div className="terms">
                    <input type="checkbox"/>
                    <div>I agree to the Releaseasy Terms and Privacy.</div>
                </div>
                <div className={"error" + (this.state.error ? " visible" : "")}></div>
                <button onClick={() => this.onRegister()}>register</button>
            </div>
        </div>;
    }

    selectType(type: RegistrationType) {
        this.setState({
            type
        });
    }

    async onRegister() {
        const result = await User.register(
            this.state.type,
            this.state.name,
            this.state.lastName,
            this.state.location,
            this.state.email,
            this.state.password,
            this.state.email
        );

        if (result === true) {
            this.setState({ error: "" });
            Application.reactApp.changePage("Login");
        }
        else {
            this.setState({ error: "There was an error while registering a new user or company." });
        }
    }
}