import type { Execution } from "../api";

type Props = {
  execution: Execution | null;
};

export function OperationResult({ execution }: Props) {
  return (
    <section
      className="card result-card"
      aria-labelledby="result-heading"
      aria-live="polite"
    >
      <div className="card-heading">
        <h2 id="result-heading">Your result</h2>
        <span className="pill">02 / OUTPUT</span>
      </div>
      {execution ? (
        <div className="result-filled">
          <span className="result-label">
            {execution.operationCode}
          </span>
          <output>
            {execution.result === ""
              ? "(empty text)"
              : execution.result}
          </output>
          <p className="result-caption">
            Your last successful calculation.
          </p>
        </div>
      ) : (
        <div className="result-empty">
          <span className="result-symbol" aria-hidden="true">
            =
          </span>
          <h3>The answer starts here</h3>
          <p>
            Enter two values and hit Calculate.
            <br />
            Your result will appear in this space.
          </p>
        </div>
      )}
    </section>
  );
}
