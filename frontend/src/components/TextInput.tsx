import { Component } from "react";
import * as React from "react";

interface TextInputProps {
    className?: string;
    placeholder?: string;
    password?: boolean;
    value: (value?: string) => string;
}

export class TextInput extends Component<TextInputProps> {
    render() {
        return <input
            type={this.props.password === true ? "password" : "text"}
            className={this.props.className}
            placeholder={this.props.placeholder}
            value={this.props.value()}
            onChange={(e) => this.props.value(e.target.value)}/>;
    }
}