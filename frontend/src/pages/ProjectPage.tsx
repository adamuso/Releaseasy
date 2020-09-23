import * as React from "react";
import { Page } from "./Page";
import Board from "react-trello";
import Sidebar from "react-sidebar";
import { Project } from "../backend/Project";
import { User } from "../backend/User";
import { EditableMarkdownDisplay } from "../components/EditableMarkdownDisplay";
import { Task } from "../backend/Task";
import { Application } from "../main";

const data = {
    lanes: [
        {
            id: 'backlog',
            title: 'Backlog',
            cards: []
        },
        {
            id: 'to-do',
            title: 'To do',
            cards: []
        },
        {
            id: 'in-progress',
            title: 'In progress',
            cards: []
        },
        {
            id: 'tests',
            title: 'Tests',
            cards: []
        },
        {
            id: 'completed',
            title: 'Completed',
            cards: []
        }
    ]
}

export class ProjectPage extends Page<{
    sidebarOpen: boolean,
    project?: Project,
    creator?: any,
    lanesData: { lanes: any[] },
    cardData: { [key: string]: { taskId: number }}
}, { id: number }> {
    constructor(props) {
        super(props);

        this.state = {
            sidebarOpen: false,
            lanesData: JSON.parse(JSON.stringify(data)),
            cardData: {}
        };
    }

    componentDidMount() {
        Project.get(this.params.id!.toString()).then(project => {
            this.setState({ project });

            User.get(project.creator).then(creator => {
                this.setState({ creator });
            });

            for (let i = 0; i < project.tasks.length; i++) {
                const task = project.tasks[i];
                const laneIndex = this.state.lanesData.lanes.findIndex(l => l.id === task.status);

                if (laneIndex < 0) {
                    continue;
                }

                const lane = this.state.lanesData.lanes[laneIndex];
                lane.cards.push({
                    id: "" + task.id,
                    title: task.name,
                    laneId: lane.id,
                    description: task.description
                });
                this.state.cardData[task.id.toString()] = { taskId: task.id };
            }

            this.forceUpdate();
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
                            if (this.state.project) {
                                if (value === undefined) {
                                    return this.state.project.description;
                                }

                                Project.put(this.params.id!.toString(), { description: value });
                                this.state.project.description = value;
                                this.forceUpdate();

                                return this.state.project.description;
                            }

                            return "";
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
                            data={this.state.lanesData}
                            style={{ backgroundColor: "initial", height: "initial", flex: "1 1 0" }}
                            editable={true}
                            draggable={true}
                            editLaneTitle={false}
                            canAddLanes={false}
                            onDataChange={(data) => {
                                this.setState({ lanesData: data });
                            }}
                            onCardAdd={async (card, lineId) => {
                                const task = await Task.create({
                                    name: card.title,
                                    description: card.description,
                                    startTime: new Date().toISOString(),
                                    status: lineId
                                });

                                this.state.cardData[card.id] = { taskId: task.id };

                                if (this.state.project) {
                                    Project.addTask(this.state.project, task);
                                }
                            }}
                            onCardMoveAcrossLanes={async (fromLaneId, toLaneId, cardId, index) => {
                                Task.edit(this.state.cardData[cardId].taskId, { status: toLaneId });
                            }}
                            onCardDelete={(cardId, lineId) => {
                                if (this.state.project) {
                                    Project.removeTask(this.state.project, this.state.cardData[cardId].taskId);
                                    //Task.delete(cardId);
                                }
                            }}
                            onCardClick={(cardId, metadata, laneId) => {
                                Application.reactApp.changePage(`Task?projectId=${this.state.project?.id}&id=${this.state.cardData[cardId].taskId}`);
                            }}/>
                    </div>
                </div>
            </Sidebar>
        </div>;
    }
}
