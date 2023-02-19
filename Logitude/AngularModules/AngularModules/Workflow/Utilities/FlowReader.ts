import { NodeType } from "Workflow/Types";
import { Formatter } from "./Formatter";

export class FlowReader {

    static getAllPreviousNodes(flowObject: any, nodeId: string, previousNodesType: NodeType | null = null) {
        if (flowObject) {
            let node = flowObject.nodes.filter((n: any) => n.id === nodeId)[0];
            if (node) {
                let allPreviousNodes = [];
                let currentNodes = [node];

                while (currentNodes.length > 1 || (currentNodes.length === 1 && currentNodes[0].type !== "startNode")) {
                    let currentPreviousNodes = [];
                    currentNodes.forEach((currentNode: any) => {
                        let previousNodes = this.getPreviousNodes(flowObject, currentNode.id);
                        currentPreviousNodes = currentPreviousNodes.concat(previousNodes);
                    });
                    currentPreviousNodes = Object.values(
                        currentPreviousNodes.reduce((previousValue, currentValue) => ({ ...previousValue, [currentValue.id]: currentValue }), {})
                    );
                    allPreviousNodes = allPreviousNodes.concat(currentPreviousNodes);
                    currentNodes = currentPreviousNodes;
                }

                allPreviousNodes = allPreviousNodes.filter((node, index, nodes) => nodes.findIndex(n => n.id === node.id) === index);

                return previousNodesType ? allPreviousNodes.filter((n: any) => n.type === previousNodesType) : allPreviousNodes;
            }
        }
        return [];
    }

    static getPreviousNodes(flowObject: any, nodeId: string) {
        if (flowObject) {
            let previousNodes = [];
            let targetEdges = flowObject.edges.filter((e: any) => e.target === nodeId);
            targetEdges.forEach((targetEdge: any) => {
                let previousNode = flowObject.nodes.filter((n: any) => n.id === targetEdge.source)[0];
                if (previousNode) {
                    previousNodes.push(previousNode);
                }
            });
            return previousNodes.filter((node, index, nodes) => nodes.findIndex(n => n.id === node.id) === index);
        }
        return [];
    }

    static getStartNode(flowObject: any) {
        if (flowObject) {
            let startNode = flowObject.nodes.filter((n: any) => n.type === "startNode")[0];
            return startNode ? startNode : null;
        }
        return null;
    }

    static getStartNodeEntity(flowObject: any) {
        if (flowObject) {
            let startNode = this.getStartNode(flowObject);
            let startNodeEntity = startNode ? startNode.data["entity"] : null;
            return startNodeEntity ? startNodeEntity : null;
        }
        return null;
    }

    static getNodes(flowObject: any, nodeType: NodeType = null) {
        if (flowObject) {
            if (nodeType) {
                return flowObject.nodes.filter((n: any) => n.type === nodeType);
            }
            return flowObject.nodes;
        }
        return [];
    }

    static isNodeCodeExists(flowObject: any, name: string) {
        if (flowObject && name) {
            return flowObject.nodes.filter((node: any) => this.isSameNodeCode(node, name)).length > 0;
        }
        return false;
    }

    static isSameNodeCode(node: any, name: string) {
        if (node && name) {
            let nodeName = node.data ? (node.data["name"] || null) : null;
            return Formatter.getCodeFromName(nodeName) === Formatter.getCodeFromName(name);
        }
        return false;
    }
}