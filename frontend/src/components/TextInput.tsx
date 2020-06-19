import { useState, useCallback, useEffect } from "react";
import * as React from "react";
import { ValidationContext } from "../utility/ValidationContext";

interface TextInputProps {
    className?: string;
    placeholder?: string;
    password?: boolean;
    validator?: (value: string) => string | true;
    validationContext?: ValidationContext;
    value: (value?: string) => string;
    multiline?: boolean;
}

export function TextInput(props: TextInputProps) {
    const [error, setError] = useState<string | undefined>(undefined);
    const [localValue, setLocalValue] = useState(props.value());

    const onValidate = useCallback(() => {
        if (props.validator) {
            const validation = props.validator(localValue);

            if (typeof validation === "string") {
                setError(validation);
                return false;
            }
            else if (validation !== true) {
                setError("Unknown error");
                return false;
            }
        }

        setError(undefined);
        return true;
    }, [props.validator, localValue]);

    useEffect(() => {
        if (props.validationContext) {
            const context = props.validationContext;
            const hook = onValidate;
            context.attach(hook);

            return () => {
                context.detach(hook)
            }
        }
    }, [props.validationContext, onValidate]);

    const onUpdateValue = (value: string) => {
        setLocalValue(value);
    };

    const onChange = () => {
        if (onValidate()) {
            props.value(localValue);
        }
    }

    return <div className="text-input">
        <div>
            {
                !props.multiline ?
                <input
                    type={props.password === true ? "password" : "text"}
                    className={props.className}
                    placeholder={props.placeholder}
                    value={localValue}
                    onBlur={() => onChange()}
                    onChange={(e) => onUpdateValue(e.target.value)}/> :
                <textarea
                    className={props.className}
                    placeholder={props.placeholder}
                    value={localValue}
                    onBlur={() => onChange()}
                    onChange={(e) => onUpdateValue(e.target.value)}/>
            }
        </div>
        <div className={"error" + (error ? " visible" : "")}>{error}</div>
    </div>;
}