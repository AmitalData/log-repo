export class FlowReader {

    static getAllPreviousNodes(flowObject: any, nodeId: string, previousNodesType: string | null = null) {
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
            return previousNodes;
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

    static getNodes(flowObject: any, type: string | null = null) {
        if (flowObject) {
            if (type) {
                return flowObject.nodes.filter((n: any) => n.type === type);
            }
            return flowObject.nodes;
        }
        return [];
    }

}