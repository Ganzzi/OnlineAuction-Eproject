'use client'

import Breadcrumb from "@/components/Dashboard/Breadcrumb";
import axiosService from "@/services/axiosService";
import { Category } from "@/types/models/category";
import { useRouter } from "next/navigation";
import { useState } from "react";


const CategoryCreateUpdatePage = () => {
  const router =  useRouter();
  const [payload, setPayload] = useState<Category>({
    categoryId: 0,
    categoryName: '',
    description: '',
  });

  const [errors, setErrors] = useState({
    message: '',
    name: '',
    description: '',
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setPayload({ ...payload, [e.target.name]: e.target.value });
  };

  const handleSubmit = async () => {

    await axiosService.post('/api/admin/AddCategory', payload)
      .then(() => {
        setErrors({
          message: '',
    name: '',
    description: '',
        });
      router.push('/dashboard/categories')
      })
      .catch((e) => {
        if (e?.response?.status === 400) {
          console.log(e?.response?.data?.errors);

          setErrors({
            message: 'Please enter fields correctly',
            name: e?.response?.data?.errors?.CategoryName ?? '',
            description: e?.response?.data?.errors?.Description ?? '',
          });
        } else if (e?.response?.status === 401) {
          setErrors({
            ...errors,
            message: '',
          });
        }
      });

  };

  return (
    <>
      <Breadcrumb pageName="Category Create" />

      <div className="rounded-sm border border-stroke bg-white shadow-default dark:border-strokedark dark:bg-boxdark">
        <div className="border-b border-stroke py-4 px-6.5 dark:border-strokedark">
          <h3 className="font-medium text-black dark:text-white">Create Category</h3>
        </div>

        <div className="flex flex-col gap-5.5 p-6.5">
          <div>
            <label className="mb-3 block text-black dark:text-white">Name</label>
            <input
              type="text"
              name="categoryName"
              placeholder="Name"
              value={payload.categoryName}
              onChange={handleChange}
              className="w-full rounded-lg border-[1.5px] border-stroke bg-transparent py-3 px-5 font-medium outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:focus:border-primary"
            />
            {errors.name && <p className="text-meta-7 text-xl">{errors.name}</p>}
          </div>

          <div>
            <label className="mb-3 block text-black dark:text-white">Description</label>
            <input
              type="text"
              name="description"
              placeholder="Description"
              value={payload.description}
              onChange={handleChange}
              className="w-full rounded-lg border-[1.5px] border-stroke bg-transparent py-3 px-5 font-medium outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:border-form-strokedark dark:bg-form-input dark:focus:border-primary"
            />
            {errors.description && <p className="text-meta-7 text-xl">{errors.description}</p>}
          </div>
        </div>

        <div className="flex flex-col gap-5.5 p-6.5">
          <div>
            <button
              onClick={handleSubmit}
              className="inline-flex items-center justify-center rounded-md bg-meta-3 py-4 px-10 text-center font-medium text-white hover:bg-opacity-90 lg:px-8 xl:px-5"
            >
              Submit
            </button>
          </div>
        </div>
      </div>
    </>
  );
};


export default CategoryCreateUpdatePage;
