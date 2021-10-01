import React from "react";
import { NavLink, Navbar, Nav } from "react-bootstrap";

const Navigation = () => {
  return (
    <Navbar bg="dark" expand="lg">
      <Navbar.Toggle aria-controls="basic-navbar-nav" />
      <Navbar.Collapse id="basic-navbar-nav">
        <Nav>
          <NavLink to="/" className="d-inline p-2 bg-dark text-white">
            Home
          </NavLink>

          <NavLink to="/department" className="d-inline p-2 bg-dark text-white">
            Department
          </NavLink>

          <NavLink to="/employee" className="d-inline p-2 bg-dark text-white">
            Employee
          </NavLink>
        </Nav>
      </Navbar.Collapse>
    </Navbar>
  );
};

export default Navigation;
