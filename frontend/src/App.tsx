import { OperationsPage } from "./pages/OperationsPage";

export default function App() {
  return (
    <div className="shell">
      <header className="topbar">
        <a className="brand" href="/" aria-label="Gaia home">
          <span className="brand-icon">g</span>gaia
          <span className="brand-dot">.</span>
        </a>
        <span className="top-note">A little space for big ideas</span>
      </header>
      <OperationsPage />
    </div>
  );
}
