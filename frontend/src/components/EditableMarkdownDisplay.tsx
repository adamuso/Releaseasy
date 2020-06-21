import { useState, useCallback, useEffect, useRef, createRef } from "react";
import * as React from "react";
import { ValidationContext } from "../utility/ValidationContext";

declare class SimpleMDE {
    constructor(options: any);

    togglePreview(): void;
}

interface EditableMarkdownDisplayProps {
    className?: string;
    placeholder?: string;
    password?: boolean;
    validator?: (value: string) => string | true;
    validationContext?: ValidationContext;
    value: (value?: string) => string;
}

export function EditableMarkdownDisplay(props: EditableMarkdownDisplayProps) {
    const [error, setError] = useState<string | undefined>(undefined);
    const [isEditing, setEditing] = useState(false);
    const [localValue, setLocalValue] = useState(props.value());
    const mdeRef = useRef<any>(undefined)

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

    useEffect(() => {
        if (isEditing) {
            const mde = new SimpleMDE({
                element: document.getElementById("markdown-editor"),
                placeholder: "Description",
                hideIcons: ["fullscreen", "side-by-side"],
                status: false
            });

            mdeRef.current = mde;
        }
        else {
            const mde = new SimpleMDE({
                element: document.getElementById("markdown-editor"),
                placeholder: "Description",
                hideIcons: ["fullscreen", "side-by-side"],
                status: false,
                toolbar: false,
                spellChecker: false
            });

            mde.togglePreview();

            mdeRef.current = mde;
        }
    }, [isEditing]);

    const onUpdateValue = (value: string) => {
        setLocalValue(value);
    };

    const onChange = () => {
        if (onValidate()) {
            props.value(localValue);
        }
    }

    return <div className="editable-markdown-display">
        <div style={{ minHeight: "100px" }}>
            {isEditing ? <div className="editor-container">
                <div className="editor-relative">
                    <textarea id="markdown-editor" style={{ display: "none" }} className={props.className} value={localValue}/>
                </div>
                <div className="buttons">
                    <button onClick={() => setEditing(false)}>Cancel</button>
                    <button onClick={() => { setEditing(false); onUpdateValue(mdeRef.current!.value()); onChange(); }} className="green">Accept</button>
                </div>
            </div> : <div key="no-edit" onDoubleClick={() => setEditing(true)}>
                <div className="editor-container">
                    <div className="editor-relative">
                        <textarea id="markdown-editor" style={{ display: "none" }} className={props.className} value={localValue}/>
                    </div>
                </div>
            </div>}
        </div>
        <div className={"error" + (error ? " visible" : "")}>{error}</div>
    </div>;
}