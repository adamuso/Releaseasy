import { Page } from "./Page";
import * as React from "react";
import { Project, ProjectCreationData } from "../backend/Project";
import { Application } from "../main";

export class CreateProjectPage extends Page<{ project: ProjectCreationData, invalidName: boolean }, { onCancelledRedirect: string, onCreatedRedirect: string }> {
    constructor(props: any) {
        super(props);

        this.state = { project: { name: "", description: "" }, invalidName: false };
    }

    render() {
        return <div>
            <div style={{ visibility: this.state.invalidName ? "visible" : "collapse" }}>Podano nieprawidłową nazwę projektu</div>
            <input type="text" value={this.state.project.name} onChange={(e) => this.updateName(e.target.value)} />
            <textarea value={this.state.project.description} onChange = {(e) => this.updateDescription(e.target.value)} />
            <button onClick={() => this.validateAndSubmit()}>Create</button>
            <button onClick={() => Application.reactApp.changePage(this.params.onCancelledRedirect!)}>Cacnel</button>
        </div>;
    }

    updateName(name: string) {
        const project = { ...this.state.project };
        project.name = name;

        this.setState({ project });

        if (!name || name.length < 3) {
            this.setState({ invalidName: true });
            return false;
        }

        this.setState({ invalidName: false });
        return true;
    }

    updateDescription(description: string) {
        const project = { ...this.state.project };
        project.description = description;

        this.setState({ project });
        return true;
    }

    async validateAndSubmit() {
        if (!this.updateName(this.state.project.name))
            return;

        if (!this.updateDescription(this.state.project.description))
            return;

        let result: Project;

        try {
             result = await Project.create(this.state.project);
        }
        catch {
            return;
        }

        Application.reactApp.changePage<Project>(this.params.onCreatedRedirect!, result);
    }
}