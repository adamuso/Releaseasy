import { Page } from "./Page";
import * as React from "react";
import { Application } from "../main";
import { CreateProjectPage } from "./CreateProjectPage";
import { App } from "../App";
import { User } from "../backend/User";
import { Project } from "../backend/Project";

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


export class UserPage extends Page<{ user?: any, createdProjects?: any[], lastCreatedProjects?: any[] }> {
    constructor(props: any) {
        super(props);

        this.state = {};
    }

    async componentDidMount() {
        User.getCurrent().then(user => {
            this.setState({ user });
        });

        User.getCreatedProjects().then(projects => {
            this.setState({ createdProjects: projects });
        });

        Project.getLastCreatedProjects().then(projects => {
            this.setState({ lastCreatedProjects: projects });
        });
    }

    render() {
        return <div className="user-page">
            <div className="top-info">
                <div className="new-project" onClick={() => Application.reactApp.changePage("CreateProject")}>Create new project</div>
                <div className="user">
                    <div className="user-info">
                        <div>
                            {
                                this.state.user ?
                                    <div>
                                        <div>{this.state.user.name}</div>
                                        <div>{this.state.user.type === "employee" ? this.state.user.lastName : this.state.user.location}</div>
                                    </div> : null
                            }
                        </div>
                        <div className="more-info">
                            <div>More</div>
                        </div>
                        <div className="user-image">
                        </div>
                    </div>
                </div>
            </div>
            <div className="projects">
                <div className="last-added">
                    <div className="title">Last added projects</div>
                    <div className="list">
                        {this.state.lastCreatedProjects ? this.state.lastCreatedProjects.map(p => <div onClick={() => Application.reactApp.changePage(`Project?id=${p.id}`)}>
                            <div className="image"></div>
                            <div className="title">{p.name}</div>
                            <div className="date">{new Date(Date.parse(p.startTime)).toLocaleString()}</div>
                        </div>) : null}
                    </div>
                    <div className="more">More...</div>
                </div>
                <div className="user-projects">
                    <div className="title">Your projects</div>
                    <div className="list">
                        {this.state.createdProjects ? this.state.createdProjects.map(p => <div onClick={() => Application.reactApp.changePage(`Project?id=${p.id}`)}>
                            <div className="image"></div>
                            <div className="title">{p.name}</div>
                            <div className="date">{new Date(Date.parse(p.startTime)).toLocaleString()}</div>
                        </div>) : null}
                    </div>
                    <div className="more">More...</div>
                </div>
            </div>
        </div>;
    }

}