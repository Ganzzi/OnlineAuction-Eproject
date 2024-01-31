import axiosService from "@/services/axiosService";
import { Category } from "@/types/models/category";
import { useRouter } from "next/navigation";
import { useState } from "react";

interface CategoryFormProps {
    category: Category;
  }
  
  const CategoryForm: React.FC<CategoryFormProps> = ({ category }) => {
    const router= useRouter()

    const {categoryId, categoryName, description} = category;
    const [payload, setPayload] = useState<Category>({ categoryId, categoryName, description });
    const [errors, setErrors] = useState({
      message: '',
      name: '',
      description: '',
    });
  
    const handleSubmit = async (e: React.FormEvent) => {
      e.preventDefault();
  
      try {
        await axiosService.post(`/api/admin/UpdateCategory/${category.categoryId}`, payload)
          .then(() => router.push('/dashboard/categories'))
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
        // Handle success, e.g., show a success message or redirect
      } catch (error) {
        // Handle error, e.g., show an error message
        console.error('Error updating category:', error);
      }
    };
  
    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
      setPayload({ ...payload, [e.target.name]: e.target.value });
    };
  
    return (
      <form onSubmit={handleSubmit}>
        <div className="flex flex-col gap-5.5 p-6.5 rounded-xl">
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
  
          <div>
            <button
              type="submit"
              className="inline-flex items-center justify-center rounded-md bg-meta-3 py-4 px-10 text-center font-medium text-white hover:bg-opacity-90 lg:px-8 xl:px-5"
            >
              Submit
            </button>
          </div>
        </div>
      </form>
    );
  };

  export default CategoryForm;