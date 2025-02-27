import React, { useState } from 'react';
import { observer } from 'mobx-react-lite';
import { vehicleModelStore } from '../stores/VehicleModelStore.js';
import { vehicleMakeStore } from '../stores/VehicleMakeStore.js';
import DataTable from '../components/DataTable.js';
import VehicleModelForm from '../forms/VehicleModelForm.js';
import Filter from '../components/Filter.js';
import Button from 'react-bootstrap/Button';
import ConfirmationModal from '../components/ConfirmModal.js';
import { Container, Row, Col } from 'react-bootstrap';
import Form from 'react-bootstrap/Form';

const VehicleModelListPage = observer(() => {
    const [editingModelId, setEditingModelId] = useState(null);
    const [isAddingNew, setIsAddingNew] = useState(false);
    const [showDeleteConfirmation, setShowDeleteConfirmation] = useState(false);
    const [modelToDeleteId, setModelToDeleteId] = useState(null);
    const [showFormModal, setShowFormModal] = useState(false);


    const handleEdit = (id) => {
        setEditingModelId(id);
        setIsAddingNew(false);
        setShowFormModal(true);
    };

    const handleCancelEdit = () => {
        setEditingModelId(null);
        setIsAddingNew(false);
        setShowFormModal(false);
    };

    const handleDelete = (id) => {
        setModelToDeleteId(id);
        setShowDeleteConfirmation(true);
    };

    const confirmDelete = () => {
        vehicleModelStore.deleteVehicleModel(modelToDeleteId);
        setShowDeleteConfirmation(false);
        setModelToDeleteId(null);
    };

    const cancelDelete = () => {
        setShowDeleteConfirmation(false);
        setModelToDeleteId(null);
    };

    const handleSave = () => {
        setEditingModelId(null);
        setIsAddingNew(false);
        setShowFormModal(false);
    };

    const handleAddNew = () => {
        setIsAddingNew(true);
        setEditingModelId(null);
        setShowFormModal(true);
    };

    const columns = [
        {
            header: 'Make',
            field: 'makeName',
            sortable: false,
            render: (model) => vehicleMakeStore.getVehicleMakeById(model.makeId)?.name,
        },
        { header: 'Name', field: 'name', sortable: true },
        { header: 'Abbreviation', field: 'abrv', sortable: true },
    ];

    const renderActions = (model) => (
        <>
            <Button variant="primary" size="sm" onClick={() => handleEdit(model.id)} className="me-2">Edit</Button>
            <Button variant="danger" size="sm" onClick={() => handleDelete(model.id)}>Delete</Button>
        </>
    );

    return (
        <Container fluid>
            <Row>
                <Col>
                    <h1>Vehicle Models</h1>
                </Col>
            </Row>

            <Row className="mb-3">
                <Col md={4}>
                    <Form.Select
                        id="makeFilter"
                        onChange={(e) => vehicleModelStore.setSelectedMakeFilterId(Number(e.target.value) || null)}
                        value={vehicleModelStore.selectedMakeFilterId || ''}
                        >
                        <option value="">All Makes</option>
                        {vehicleMakeStore.vehicleMakes.map(make => (
                            <option key={make.id} value={make.id}>
                                {make.name}
                            </option>
                        ))}
                    </Form.Select>
                </Col>
                <Col md={4}>
                    <Filter
                        filterText={vehicleModelStore.filterText}
                        onFilterChange={vehicleModelStore.setFilterText}
                        placeholder="Filter by name or abbreviation"
                    />
                </Col>
                <Col md="auto">
                    <Button variant="success" onClick={handleAddNew}>Add New Vehicle Model</Button>
                </Col>
            </Row>


            <Row>
                <Col>
                    <DataTable
                        data={vehicleModelStore.pagedVehicleModels}
                        columns={columns}
                        sortColumn={vehicleModelStore.sortColumn}
                        sortDirection={vehicleModelStore.sortDirection}
                        onSort={vehicleModelStore.setSortColumn}
                        currentPage={vehicleModelStore.currentPage}
                        totalPages={vehicleModelStore.totalPages}
                        pageSize={vehicleModelStore.pageSize}
                        onPageChange={vehicleModelStore.setCurrentPage}
                        onPageSizeChange={vehicleModelStore.setPageSize}
                        renderActions={renderActions}
                    />
                </Col>
            </Row>

            <ConfirmationModal
                show={showFormModal}
                onHide={handleCancelEdit}
                title={isAddingNew ? "Add New Vehicle Model" : "Edit Vehicle Model"}
                body={
                    <VehicleModelForm
                        modelId={editingModelId}
                        onSave={handleSave}
                        onCancel={handleCancelEdit}
                        makeStore={vehicleMakeStore}
                        modelStore={vehicleModelStore}
                    />
                }
                onConfirm={null}
                showCancelButton={false}
                cancelText="Close"
            />

            <ConfirmationModal
                show={showDeleteConfirmation}
                onHide={cancelDelete}
                title="Delete Confirmation"
                body="Are you sure you want to delete this Vehicle Model?"
                onConfirm={confirmDelete}
                onCancel={cancelDelete}
                confirmText="Delete"
                cancelText="Cancel"
                isConfirmation={true}
            />
        </Container>
    );
});

export default VehicleModelListPage;