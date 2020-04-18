import { Page } from "./Page";
import React = require("react");

export class RegisterPage extends Page<{ type: string | "employee" | "company" }> {
    state = {
        type: "employee"
    };

    constructor(props: {}) {
        super(props);
    }

    render() {
        return <div className="register-page">
            <div className="register-form">
                <div className="type">
                    <div onClick={() => this.selectType("employee")} className={this.state.type === "employee" ? "selected" : ""}>Employee</div>
                    <div onClick={() => this.selectType("company")} className={this.state.type === "company" ? "selected" : ""}>Company</div>
                </div>
                {this.state.type === "employee" ? <div className="data">
                    <input placeholder="name"/>
                    <input placeholder="last name"/>
                    <input placeholder="email"/>
                    <input type="password" placeholder="password"/>
                    <input type="password" placeholder="confirm password"/>
                </div> : <div className="data">
                    <input placeholder="name"/>
                    <input placeholder="location"/>
                    <input placeholder="email"/>
                    <input type="password" placeholder="password"/>
                    <input type="password" placeholder="confirm password"/>
                </div>}
                <div>I agree to the Releaseasy Terms and Privacy.</div>
                <input type="checkbox"/>
                <input type="submit" value="register"/>
            </div>
        </div>;
    }

    selectType(type: "employee" | "company") {
        this.setState({
            type
        });
    }

    onRegister() {

    }
}