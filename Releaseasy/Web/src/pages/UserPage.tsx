import { Page } from "./Page";
import * as React from "react";
import { Application } from "../main";
import { CreateProjectPage } from "./CreateProjectPage";

class ProjectList extends React.Component {
    render() {
        return null;
    }
}

class TaskList extends React.Component {
    render() {
        return null;
    }
}

class Avatar extends React.Component {
    render() {
        return null;
    }
}

function LeftPanelOption(props: { title?: string, text: string, onClick: () => void }) {
    return <div>
        {props.title ? (
            <div>
                {props.title}
            </div>
        ) : null}
        <div onClick={props.onClick}>
            {props.text}
        </div>
    </div>;
}

function LeftPanel(props: { onOptionExecuted: (option: string) => void }) {
    return <div>
        <div>
            <Avatar />
            <div>
                <div />
                <div />
            </div>
        </div>
        <LeftPanelOption text="Utwórz" title="Utwórz nowy projekt" onClick={() => props.onOptionExecuted("createProject")} />
    </div>;
}

function RightPanel() {
    return <div>
        <div>Moje projekty</div>
        <ProjectList/>
        <div>Moje zadania</div>
        <TaskList/>
    </div>;
}


export class UserPage extends Page {
    constructor(props: {}) {
        super(props);

    }

    render() {
        return <div>
            <LeftPanel onOptionExecuted={() => Application.reactApp.change(CreateProjectPage, "CreateProject", { onCancelledRedirect: "Dashboard", onCreatedRedirect: "Project" })} />
            <RightPanel />
        </div>;
    }

}