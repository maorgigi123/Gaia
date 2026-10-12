import { useState } from "react";
import { api, type History } from "../api";

type Props = {
  operationCode: string;
  initialHistory: History;
};

export function ExecutionHistory({ operationCode, initialHistory }: Props) {
  const [history, setHistory] = useState(initialHistory);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  async function loadPage(page: number) {
    setLoading(true);
    setError("");
    try {
      setHistory(await api<History>(
        `/${encodeURIComponent(operationCode)}/history?page=${page}&pageSize=${history.pageSize}`,
      ));
    } catch (e) {
      setError((e as Error).message);
    } finally {
      setLoading(false);
    }
  }

  return (
    <section className="card history" aria-labelledby="history-heading">
      <div className="history-heading">
        <div>
          <p className="eyebrow">A LITTLE PERSPECTIVE</p>
          <h2 id="history-heading">
            {operationCode} execution history
          </h2>
        </div>
        <div className="month-stat">
          <strong>{history.currentMonthCount}</strong>
          <span>
            {new Date(history.monthStartUtc).toLocaleDateString(undefined, {
              month: "short", year: "numeric", timeZone: "UTC",
            })} · UTC
          </span>
        </div>
      </div>
      <p className="hint">
        {history.totalCount} successful executions · newest first
      </p>
      <div className="table-scroll">
        <table aria-busy={loading}>
          <thead>
            <tr>
              <th>Field A</th>
              <th>Field B</th>
              <th>Result</th>
              <th>Executed</th>
            </tr>
          </thead>
          <tbody>
            {history.items.map((item) => (
              <tr key={item.id}>
                <td>{item.fieldA || "(empty)"}</td>
                <td>{item.fieldB || "(empty)"}</td>
                <td className="history-result">
                  {item.result || "(empty)"}
                </td>
                <td>{new Date(item.executedAtUtc).toLocaleString()}</td>
              </tr>
            ))}
            {!history.items.length && (
              <tr><td colSpan={4}>No executions on this page.</td></tr>
            )}
          </tbody>
        </table>
      </div>
      <nav className="pagination" aria-label="History pages">
        <button
          type="button"
          disabled={loading || history.page <= 1}
          onClick={() => void loadPage(history.page - 1)}
        >
          Previous
        </button>
        <span role="status">
          {loading ? "Loading…" : history.totalPages === 0
            ? "No history yet"
            : `Page ${history.page} of ${history.totalPages}`}
        </span>
        <button
          type="button"
          disabled={loading || history.page >= history.totalPages}
          onClick={() => void loadPage(history.page + 1)}
        >
          Next
        </button>
      </nav>
      {error && <div className="error" role="alert">{error}</div>}
    </section>
  );
}
