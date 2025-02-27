import { makeAutoObservable, runInAction } from 'mobx';

const API_BASE = 'https://localhost:7224/api/VehicleMake';

class VehicleMakeStore {
    vehicleMakes = [];
    filterText = "";
    sortColumn = null;
    sortDirection = 'asc';
    currentPage = 1;
    pageSize = 5;
    error = null;

    constructor() {
        makeAutoObservable(this);
        this.fetchVehicleMakes();
    }

    async fetchVehicleMakes() {
        this.loading = true;
        this.error = null;
        try {
            const response = await fetch(API_BASE);
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const data = await response.json();
            runInAction(() => {
                this.vehicleMakes = data;
            });
        } catch (error) {
            runInAction(() => {
                this.error = error;
            });
            console.error("Error fetching vehicle makes:", error);
        }
    }

    get filteredVehicleMakes() {
        const filterRegex = new RegExp(this.filterText, 'i');
        return this.vehicleMakes.filter(make =>
            filterRegex.test(make.name) || filterRegex.test(make.abrv)
        );
    }

    get sortedVehicleMakes() {
        if (!this.sortColumn) {
            return this.filteredVehicleMakes;
        }
        const sorted = [...this.filteredVehicleMakes].sort((a, b) => {
            const aValue = a[this.sortColumn] || '';
            const bValue = b[this.sortColumn] || '';
            if (typeof aValue === 'string' && typeof bValue === 'string') {
                return this.sortDirection === 'asc' ? aValue.localeCompare(bValue) : bValue.localeCompare(aValue);
            }
            return this.sortDirection === 'asc' ? (aValue > bValue ? 1 : -1) : (bValue > aValue ? 1 : -1);
        });
        return sorted;
    }

    get pagedVehicleMakes() {
        const startIndex = (this.currentPage - 1) * this.pageSize;
        return this.sortedVehicleMakes.slice(startIndex, startIndex + this.pageSize);
    }

    get totalPages() {
        return Math.ceil(this.sortedVehicleMakes.length / this.pageSize);
    }

    async addVehicleMake(make) {
        try {
            const response = await fetch(API_BASE, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(make)
            });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const data = await response.json();
            runInAction(() => {
                this.vehicleMakes.push(data);
            });
        } catch (error) {
            console.error("Error adding vehicle make:", error);
        }
    }

    async updateVehicleMake(updatedMake) {
        try {
            const response = await fetch(`${API_BASE}/${updatedMake.id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(updatedMake)
            });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            runInAction(() => {
                this.vehicleMakes = this.vehicleMakes.map(make =>
                    make.id === updatedMake.id ? updatedMake : make
                );
            });
        } catch (error) {
            console.error("Error updating vehicle make:", error);
        }
    }

    async deleteVehicleMake(id) {
        try {
            const response = await fetch(`${API_BASE}/${id}`, {
                method: 'DELETE'
            });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            runInAction(() => {
                this.vehicleMakes = this.vehicleMakes.filter(make => make.id !== id);
            });
        } catch (error) {
            console.error("Error deleting vehicle make:", error);
        }
    }

    setFilterText = (text) => {
        this.filterText = text;
        this.currentPage = 1;
    };

    setSortColumn = (column) => {
        if (this.sortColumn === column) {
            this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
        } else {
            this.sortColumn = column;
            this.sortDirection = 'asc';
        }
    };

    setCurrentPage = (page) => {
        this.currentPage = page;
    };

    setPageSize = (size) => {
        this.pageSize = size;
        this.currentPage = 1;
    };

    getVehicleMakeById = (id) => {
        return this.vehicleMakes.find(make => make.id === id);
    };

}

export const vehicleMakeStore = new VehicleMakeStore();