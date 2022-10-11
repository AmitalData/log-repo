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

}