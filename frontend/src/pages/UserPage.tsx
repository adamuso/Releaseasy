import { Page } from "./Page";
import * as React from "react";
import { Application } from "../main";
import { CreateProjectPage } from "./CreateProjectPage";
import { App } from "../App";

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
        return <div className="user-page">
            <div className="top-info">
                <div className="new-project" onClick={() => Application.reactApp.changePage("CreateProject")}>Create new project</div>
                <div className="user">
                    <div className="user-info">
                        <div>
                            <div>John Smith</div>
                            <div>Company name</div>
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
                        <div>
                            <div className="image"></div>
                            <div className="title">Projekt 1</div>
                            <div className="date">19.04.2020</div>
                        </div>
                        <div>
                            <div className="image"></div>
                            <div className="title">Projekt 2</div>
                            <div className="date">19.04.2020</div>
                        </div>
                        <div>
                            <div className="image"></div>
                            <div className="title">Projekt 3</div>
                            <div className="date">19.04.2020</div>
                        </div>
                        <div>
                            <div className="image"></div>
                            <div className="title">Projekt 4</div>
                            <div className="date">19.04.2020</div>
                        </div>
                        <div>
                            <div className="image"></div>
                            <div className="title">Projekt 5</div>
                            <div className="date">19.04.2020</div>
                        </div>
                    </div>
                    <div className="more">More...</div>
                </div>
                <div className="user-projects">
                    <div className="title">Your projects</div>
                    <div className="list">
                        <div>
                            <div className="image"></div>
                            <div className="title">Projekt 1</div>
                            <div className="date">19.04.2020</div>
                        </div>
                        <div>
                            <div className="image"></div>
                            <div className="title">Projekt 2</div>
                            <div className="date">19.04.2020</div>
                        </div>
                        <div>
                            <div className="image"></div>
                            <div className="title">Projekt 3</div>
                            <div className="date">19.04.2020</div>
                        </div>
                        <div>
                            <div className="image"></div>
                            <div className="title">Projekt 4</div>
                            <div className="date">19.04.2020</div>
                        </div>
                        <div>
                            <div className="image"></div>
                            <div className="title">Projekt 5</div>
                            <div className="date">19.04.2020</div>
                        </div>
                    </div>
                    <div className="more">More...</div>
                </div>
            </div>
        </div>;
    }

}