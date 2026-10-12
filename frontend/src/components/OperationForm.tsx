import { useEffect, useState, type FormEvent } from "react";
import { api, type Execution } from "../api";

type Props = {
  onExecutionChange: (execution: Execution) => void;
};

export function OperationForm({ onExecutionChange }: Props) {
  const [operations, setOperations] = useState<string[]>([]);
  const [operationCode, setOperationCode] = useState("");
  const [fieldA, setFieldA] = useState("");
  const [fieldB, setFieldB] = useState("");
  const [loading, setLoading] = useState(true);
  const [busy, setBusy] = useState(false);
  const [error, setError] = useState("");

  async function loadOperations() {
    setLoading(true);
    setError("");
    try {
      const list = await api<string[]>("");
      setOperations(list);
      setOperationCode(list[0] ?? "");
      if (!list.length) setError("No operations are currently available.");
    } catch (e) {
      setError((e as Error).message);
    } finally {
      setLoading(false);
    }
  }
  useEffect(() => {
    void loadOperations();
  }, []);

  async function calculate(event: FormEvent) {
    event.preventDefault();
    setBusy(true);
    setError("");
    try {
      onExecutionChange(
        await api<Execution>("/execute", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({ operationCode, fieldA, fieldB }),
        }),
      );
    } catch (e) {
      setError((e as Error).message);
    } finally {
      setBusy(false);
    }
  }

  return (
    <section
      className="card calculator"
      aria-labelledby="calculate-heading"
    >
      <div className="card-heading">
        <h2 id="calculate-heading">Make a calculation</h2>
        <span className="pill">01 / INPUT</span>
      </div>
      <form onSubmit={calculate}>
        <fieldset disabled={busy || loading || !operations.length}>
          <label htmlFor="field-a">Field A</label>
          <input
            id="field-a"
            value={fieldA}
            maxLength={1000}
            placeholder="Enter your first value"
            onChange={(e) => {
              setFieldA(e.target.value);
              setError("");
            }}
          />
          <label htmlFor="operation">Operation</label>
          <select
            id="operation"
            value={operationCode}
            onChange={(e) => {
              setOperationCode(e.target.value);
              setError("");
            }}
          >
            {loading ? (
              <option>Loading operations…</option>
            ) : (
              operations.map((o) => (
                <option key={o} value={o}>
                  {o}
                </option>
              ))
            )}
          </select>
          <label htmlFor="field-b">Field B</label>
          <input
            id="field-b"
            value={fieldB}
            maxLength={1000}
            placeholder="Enter your second value"
            onChange={(e) => {
              setFieldB(e.target.value);
              setError("");
            }}
          />
          <p className="hint">
            Use a dot for decimals, such as 12.5. Text is kept as entered.
          </p>
          <button className="primary" type="submit">
            {busy ? "Calculating…" : "Calculate"}
            <span aria-hidden="true">↗</span>
          </button>
        </fieldset>
      </form>
      {error && (
        <div className="error" role="alert">
          {error}
        </div>
      )}
      {!loading && !operations.length && (
        <button className="retry" onClick={() => void loadOperations()}>
          Retry connection
        </button>
      )}
    </section>
  );
}
