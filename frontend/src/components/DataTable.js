import React from 'react';
import Table from 'react-bootstrap/Table';
import PagingControls from './PagingControls';

const DataTable = ({
    data,
    columns,
    sortColumn,
    sortDirection,
    onSort,
    currentPage,
    totalPages,
    pageSize,
    onPageChange,
    onPageSizeChange,
    renderActions,
}) => {
    return (
        <div>
            <Table striped bordered hover responsive>
                <thead>
                    <tr>
                        {columns.map(column => (
                            <th
                                key={column.field}
                                onClick={column.sortable ? () => onSort(column.field) : undefined}
                                style={{ cursor: column.sortable ? 'pointer' : 'default' }}
                            >
                                {column.header}
                                {column.sortable && sortColumn === column.field && (sortDirection === 'asc' ? ' ↑' : ' ↓')}
                            </th>
                        ))}
                        {renderActions && <th>Actions</th>}
                    </tr>
                </thead>
                <tbody>
                    {data.map(item => (
                        <tr key={item.id}>
                            {columns.map(column => (
                                <td key={`${item.id}-${column.field}`}>
                                    {column.render ? column.render(item) : item[column.field]}
                                </td>
                            ))}
                            {renderActions && <td>{renderActions(item)}</td>}
                        </tr>
                    ))}
                </tbody>
            </Table>
            <div style={{ marginTop: '10px' }} className="d-flex justify-content-between align-items-center">
                <PagingControls
                    currentPage={currentPage}
                    totalPages={totalPages}
                    pageSize={pageSize}
                    onPageChange={onPageChange}
                    onPageSizeChange={onPageSizeChange}
                />
            </div>
        </div>
    );
};

export default DataTable;