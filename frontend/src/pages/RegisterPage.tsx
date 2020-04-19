import { Page } from "./Page";
import React = require("react");
import { TextInput } from "../components/TextInput";
import { User } from "../backend/User";
import { Application } from "../main";

type RegistrationType = "employee" | "company";

interface RegisterPageState {
    type: RegistrationType;
    name: string;
    lastName: string;
    location: string;
    email: string;
    password: string;
    confirmPassword: string;
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
    };

    constructor(props: {}) {
        super(props);
    }

    render() {
        const $ = this.binder;

        return <div className="register-page">
            <div className="register-form">
                <div className="type">
                    <div onClick={() => this.selectType("employee")} className={this.state.type === "employee" ? "selected" : ""}>Employee</div>
                    <div onClick={() => this.selectType("company")} className={this.state.type === "company" ? "selected" : ""}>Company</div>
                </div>
                {this.state.type === "employee" ? <div className="data">
                    <TextInput placeholder="name" value={$("name")}/>
                    <TextInput placeholder="last name" value={$("lastName")}/>
                    <TextInput placeholder="email" value={$("email")}/>
                    <TextInput password={true} placeholder="password" value={$("password")}/>
                    <TextInput password={true} placeholder="confirm password" value={$("confirmPassword")}/>
                </div> : <div className="data">
                    <TextInput placeholder="name" value={$("name")}/>
                    <TextInput placeholder="location" value={$("location")}/>
                    <TextInput placeholder="email" value={$("email")}/>
                    <TextInput password={true} placeholder="password" value={$("password")}/>
                    <TextInput password={true} placeholder="confirm password" value={$("confirmPassword")}/>
                </div>}
                <div>I agree to the Releaseasy Terms and Privacy.</div>
                <input type="checkbox"/>
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
            Application.reactApp.changePage("Login");
        }
    }
}