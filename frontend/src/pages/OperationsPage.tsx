import { useState } from "react";
import type { Execution } from "../api";
import { OperationForm } from "../components/OperationForm";
import { OperationResult } from "../components/OperationResult";
import { ExecutionHistory } from "../components/ExecutionHistory";

export function OperationsPage() {
  const [execution, setExecution] = useState<Execution | null>(null);
  const [executionVersion, setExecutionVersion] = useState(0);

  function handleExecutionChange(next: Execution) {
    setExecution(next);
    // Reset pagination only after success; keep the existing content while waiting.
    setExecutionVersion((version) => version + 1);
  }

  return (
    <main>
      <div className="intro">
        <p className="eyebrow">THE OPERATIONS WORKSPACE</p>
        <h1>
          Two inputs.
          <br />
          <span>More possibilities.</span>
        </h1>
        <p>
          Work with numbers and words. Choose an operation,
          <br className="desktop-break" /> add your values, and let Gaia do
          the rest.
        </p>
      </div>
      <div className="workspace">
        <OperationForm onExecutionChange={handleExecutionChange} />
        <OperationResult execution={execution} />
      </div>
      {execution && (
        <ExecutionHistory
          key={executionVersion}
          operationCode={execution.operationCode}
          initialHistory={execution.history}
        />
      )}
    </main>
  );
}
