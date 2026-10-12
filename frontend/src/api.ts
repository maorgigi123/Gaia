export type HistoryEntry = {
  id: number;
  fieldA: string;
  fieldB: string;
  result: string;
  executedAtUtc: string;
};
export type History = {
  items: HistoryEntry[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  currentMonthCount: number;
  monthStartUtc: string;
};
export type Execution = {
  operationCode: string;
  result: string;
  history: History;
};
type ApiResponse<T> =
  | { success: true; data: T; error: null }
  | { success: false; data: null; error: { code: string; message: string } };

export async function api<T>(path: string, init?: RequestInit): Promise<T> {
  let response: Response;
  try {
    response = await fetch(`/api/v1/operations${path}`, init);
  } catch {
    throw new Error("Cannot reach Gaia. Check your connection and try again.");
  }
  let body: ApiResponse<T>;
  try {
    body = await response.json();
  } catch {
    throw new Error(
      "The server returned an unexpected response. Please try again.",
    );
  }
  if (!body.success) throw new Error(body.error.message);
  if (!response.ok) throw new Error("The request failed. Please try again.");
  return body.data;
}
