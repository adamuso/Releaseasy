import { Page } from "./Page";
import * as React from "react";
import { Project, ProjectCreationData } from "../backend/Project";
import { Application } from "../main";
import { TextInput } from "../components/TextInput";
import { ValidationContext } from "../utility/ValidationContext";

export class CreateProjectPage extends Page<{ invalidName: boolean } & ProjectCreationData, { onCancelledRedirect: string, onCreatedRedirect: string }> {
    private validationContext = new ValidationContext();

    constructor(props: any) {
        super(props);

        this.state = { name: "", description: "", invalidName: false };
    }

    render() {
        const $ = this.binder;

        const nameValidator = (message: string) => {
            return (name: string) => {
                if (!name || name.length < 3) {
                    return message;
                }

                return true;
            };
        };

        const cancelRedirect = this.params.onCancelledRedirect ?? "User";

        return <div className="create-project-page">
            <div className="form">
                <div className="title">
                    Create a new project
                </div>
                <TextInput placeholder="name" value={$("name")} validator={nameValidator("Specified name is invalid.")} validationContext={this.validationContext} />
                <TextInput multiline placeholder="description" value={$("description")} validationContext={this.validationContext} />
                <div className="buttons">
                    <button onClick={() => this.validateAndSubmit()}>Create</button>
                    <button onClick={() => Application.reactApp.changePage(cancelRedirect)}>Cancel</button>
                </div>
            </div>
        </div>;
    }

    updateDescription(description: string) {
        this.setState({ description });
        return true;
    }

    async validateAndSubmit() {
        if (!this.validationContext.validate()) {
            return;
        }

        let result: Project;

        try {
             result = await Project.create({ name: this.state.name, description: this.state.description });
        }
        catch {
            return;
        }

        const createdRedirect = this.params.onCreatedRedirect ?? "User";

        Application.reactApp.changePage<Project>(createdRedirect, result);
    }
}