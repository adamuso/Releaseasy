import * as React from "react";
import { Page } from "./Page";
import Board from "react-trello";
import Sidebar from "react-sidebar";
import { Project } from "../backend/Project";
import { User } from "../backend/User";
import { EditableMarkdownDisplay } from "../components/EditableMarkdownDisplay";

const data = {
    lanes: [
        {
            id: 'lane1',
            title: 'Backlog',
            cards: [
                { id: 'Card1', title: 'Create front-end', description: 'Create front end for application', label: '' },
                { id: 'Card2', title: 'Create back-end', description: 'Create back end for application', label: '' },
                { id: 'Card3', title: 'Tests', description: 'Test the application', label: '5 mins' }
            ]
        },
        {
            id: 'lane2',
            title: 'To do',
            cards: []
        },
        {
            id: 'lane3',
            title: 'In progress',
            cards: []
        },
        {
            id: 'lane4',
            title: 'Tests',
            cards: []
        },
        {
            id: 'lane5',
            title: 'Completed',
            cards: []
        }
    ]
}

export class ProjectPage extends Page<{ sidebarOpen: boolean, project?: any, creator?: any }, { id: number }> {
    constructor(props) {
        super(props);

        this.state = {
            sidebarOpen: false
        };
    }

    componentDidMount() {
        Project.get(this.params.id!.toString()).then(project => {
            this.setState({ project });

            User.get(project.creator).then(creator => {
                this.setState({ creator });
            });
        });
    }

    onSetSidebarOpen(open: boolean) {
        this.setState({ sidebarOpen: open });
    }

    render() {
        const sidebarContent = <div className="sidebar">
            {this.state.project ? <div>
                <div className="project-title">Project {this.state.project.name}</div>
                <div>
                    <div className="project-description">Description</div>
                    <div className="project-description-content">
                        {/* {this.state.project.description} */}
                        <EditableMarkdownDisplay value={(value?: string) => {
                            if (value === undefined) {
                                return this.state.project.description;
                            }

                            Project.put(this.params.id!.toString(), { description: value });
                            this.state.project.description = value;
                            this.forceUpdate();
                        }}/>
                    </div>
                </div>
                <div>
                    <div className="project-tags">Tags</div>
                    <div className="project-tags-content">
                    </div>
                </div>
                <div>
                    <div className="project-assigned">Assigned</div>
                    <div className="project-assigned-content">
                    </div>
                </div>
                <button className="assign-to">Assign to</button>
                {this.state.creator ? <div>
                    Created by {this.state.creator.name} {this.state.creator.lastName} {this.state.creator.location}
                </div> : null}

            </div> : null}
        </div>

        return <div className="project-page">
            <Sidebar
                sidebar={sidebarContent}
                open={this.state.sidebarOpen}
                onSetOpen={(v) => this.onSetSidebarOpen(v)}
                styles={{ sidebar: { background: "white" } }}
                docked={this.state.sidebarOpen}>
                <div className="board-container">
                    <button className="expand-sidebar" onClick={() => this.onSetSidebarOpen(!this.state.sidebarOpen)}>
                        {this.state.sidebarOpen ? "<" : ">"}
                    </button>
                    <div className="board">
                        <Board
                            data={data}
                            style={{ backgroundColor: "initial", height: "initial", flex: "1 1 0" }}
                            editable={true}
                            draggable={true}
                            editLaneTitle={false}
                            canAddLanes={false}/>
                    </div>
                </div>
            </Sidebar>
        </div>;
    }
}
