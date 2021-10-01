import React, { useCallback, useEffect, useState } from "react";
import { Table } from "react-bootstrap";

const Department = () => {
  const [deps, setDeps] = useState([]);

  const refreshList = useCallback(async () => {
    const fetched = await fetch(process.env.REACT_APP_API + "department");
    const jsonRes = await fetched.json();
    setDeps(jsonRes);
  }, [setDeps]);

  useEffect(() => {
    refreshList();
  }, [refreshList]);

  return (
    <div>
      <Table className="mt-4" striped bordered hover size="sm">
        <thead>
          <tr>
            <th>DepartmentId</th>
            <th>DepartmentName</th>
            <th>Options</th>
          </tr>
        </thead>
        <tbody>
          {deps.map((dep) => (
            <tr key={dep.DepartmentId}>
              <td>{dep.DepartmentId}</td>
              <td>{dep.DepartmentName}</td>
              <td>Edit / Delete</td>
            </tr>
          ))}
        </tbody>
      </Table>
    </div>
  );
};

export default Department;
