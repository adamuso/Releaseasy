﻿import * as React from "react";
import { Page } from "./Page";
import Board from "react-trello";
import Sidebar from "react-sidebar";
import { createGlobalStyle } from "styled-components";
import { Project } from "../backend/Project";
import { User } from "../backend/User";

const data = {
    lanes: [
        {
            id: 'lane1',
            title: 'Backlog',
            cards: [
                { id: 'Card1', title: 'Write Blog', description: 'Can AI make memes', label: '30 mins', draggable: false },
                { id: 'Card2', title: 'Pay Rent', description: 'Transfer via NEFT', label: '5 mins', metadata: { sha: 'be312a1' } }
            ]
        },
        {
            id: 'lane1',
            title: 'Planned Tasks',
            label: '2/2',
            cards: [
                { id: 'Card1', title: 'Write Blog', description: 'Can AI make memes', label: '30 mins', draggable: false },
                { id: 'Card2', title: 'Pay Rent', description: 'Transfer via NEFT', label: '5 mins', metadata: { sha: 'be312a1' } }
            ]
        },
        {
            id: 'lane2',
            title: 'Completed',
            label: '0/0',
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
        const sidebarContent = <div>
            {this.state.project ? <div>
                <div>Project {this.state.project.name}</div>
                <div>Description</div>
                <div>
                    {this.state.project.description}
                </div>
                <div>
                    <div>Tags</div>
                </div>
                <div>
                    <div>Assigned</div>
                </div>
                <button>Assign to</button>
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
                <div>
                    <button onClick={() => this.onSetSidebarOpen(!this.state.sidebarOpen)}>
                        {this.state.sidebarOpen ? "<" : ">"}
                    </button>
                    <div>
                        <Board data={data} style={{ backgroundColor: "initial" }} editable={true} draggable={true} editLaneTitle={true} canAddLanes={true}/>
                    </div>
                </div>
            </Sidebar>
        </div>;
    }
}
