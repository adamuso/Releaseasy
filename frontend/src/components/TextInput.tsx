import { Component } from "react";
import * as React from "react";

interface ValidationContext {

}

interface TextInputProps {
    className?: string;
    placeholder?: string;
    password?: boolean;
    validator?: (value: string) => string | true;
    validationContext?: ValidationContext;
    value: (value?: string) => string;
}

export class TextInput extends Component<TextInputProps, { error?: string, localValue: string }> {
    static getDerivedStateFromProps(nextProps: TextInputProps, prevState: { error?: string, localValue: string }){
        const result: Partial<{ error?: string, localValue: string }> = {};

        if(nextProps.value() !== prevState.localValue){
            result.localValue = nextProps.value();
        }

        return result;
     }

    constructor(props: TextInputProps, context: any) {
        super(props, context);
        this.state = {
            localValue: props.value()
        };
    }

    render() {
        return <div className="text-input">
            <div>
                <input
                    type={this.props.password === true ? "password" : "text"}
                    className={this.props.className}
                    placeholder={this.props.placeholder}
                    value={this.state.localValue}
                    onBlur={() => this.onChange()}
                    onChange={(e) => this.onUpdateValue(e.target.value)}/>
            </div>
            <div className={"error" + (this.state.error ? " visible" : "")}>{this.state.error}</div>
        </div>;
    }

    private onUpdateValue(value: string) {
        this.setState({ localValue: value });
    }

    private onChange() {
        if (this.props.validator) {
            const validation = this.props.validator(this.state.localValue);

            if (typeof validation === "string") {
                this.setState({ error: validation });
                return;
            }
            else if (validation !== true) {
                this.setState({ error: undefined });
                return;
            }
        }

        this.setState({ error: undefined });

        this.props.value(this.state.localValue);
    }
}