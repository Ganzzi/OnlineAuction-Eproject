'use client'

import axiosService from "@/services/axiosService"
import FileUpload from "@/components/common/FileUpload/FileUploadOne"
import { useGlobalState } from "@/context/globalState"
import { Category } from "@/types/models/category"
import { Item } from "@/types/models/item"
import { useEffect, useState } from "react"
import { MultiSelect, Option } from "react-multi-select-component"
import { useRouter } from "next/navigation"

type PageProps = {
  item?: Item,
  existedCategories?: Category[],
  categories: Category[]
}

type SellItemPayload = {
  item: Item,
  categories: Category[]
}

const Index: React.FC<PageProps> = ({ item, categories, existedCategories }) => {
  const { user, isLoggedIn } = useGlobalState();
  const router = useRouter();

  const initialFormData: SellItemPayload = {
    item: {
      itemId: 0,
      title: '',
      description: '',
      image: 'null',
      imageFile: undefined,
      document: 'null ',
      documentFile: undefined,
      startingPrice: 0,
      increasingAmount: 0,
      reservePrice: undefined,
      // sellerId: user.userId,
      sellerId: undefined,
      seller: undefined,
      categoryItems: undefined,
      bids: undefined,
      startDate: new Date().toISOString().split('T')[0], // Set initial start date to today
      endDate: new Date().toISOString().split('T')[0],   // Set initial end date to today
    },
    categories: [],
  };

  const [formData, setFormData] = useState<SellItemPayload>(initialFormData);
  const [selectedCategories, setSelectedCategories] = useState<Option[]>([]);
  const [resMessage, setResMessage] = useState({
    title: "",
    description: "",
    image: "",
    document: "",
    categories: "",
    content: "",
    color: ""
  });

  useEffect(() => {
    if (item) {
      setFormData((prevFormData) => ({
        ...prevFormData,
        item: {
          ...prevFormData.item,
          ...item,
          startingPrice: prevFormData.item.startingPrice !== 0 ? prevFormData.item.startingPrice : item.startingPrice,
          increasingAmount: prevFormData.item.increasingAmount !== 0 ? prevFormData.item.increasingAmount : item.increasingAmount,
        },
      }));
    } if (existedCategories) {
      setSelectedCategories(existedCategories.map(cate => ({
        value: cate.categoryId,
        label: cate.categoryName,
        key: cate.description
      })))
    }
  }, [])

  const handleImageChange = (file: File) => {
    setFormData((prevFormData) => ({
      ...prevFormData,
      item: {
        ...prevFormData.item,
        imageFile: file,
      },
    }));
  };
  const handleDocumentChange = (file: File) => {
    setFormData((prevFormData) => ({
      ...prevFormData,
      item: {
        ...prevFormData.item,
        documentFile: file,
      },
    }));
  };


  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    if (!isLoggedIn) {
      router.push("/auth/signin")
    }

    try {
      const formDataToSend = new FormData();

      // Append item fields to FormData
      Object.entries(formData.item).forEach(([key, value]) => {
        // Check for undefined values before appending to FormData
        if (value !== undefined && value !== null) {
          formDataToSend.append(`item.${key}`, value.toString());
        }
      });



      // Append imageFile to FormData
      if (formData.item.imageFile) {
        formDataToSend.append('item.ImageFile', formData.item.imageFile);
      }

      if (formData.item.documentFile) {
        console.log('has document');

        formDataToSend.append('item.DocumentFile', formData.item.documentFile);
      }

      selectedCategories.forEach((category, index) => {
        formDataToSend.append(`categories[${index}].categoryId`, category.value.toString());
        formDataToSend.append(`categories[${index}].categoryName`, category.label);
        formDataToSend.append(`categories[${index}].Description`, category?.key ?? "");
      });
      console.log(Array.from(formDataToSend));

      await axiosService.post(`/api/user/${item == undefined ? "sellItem" : "updateItem"}`, formDataToSend, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      }).then((res) => {
        if (res.status == 200) {
          setResMessage({
            ...resMessage,
            content: res.data?.message,
            color: "meta-4"
          })

          router.push("/items/" + res.data?.itemId);
        }
      }).catch((e) => {
        if (e?.response?.status === 400) {
          const errors = e?.response?.data?.errors;

          setResMessage({
            content: e?.response?.data?.message,
            title: errors?.['Item.Title'] ? errors['Item.Title'][0] : '',
            description: errors?.['Item.Description'] ? errors['Item.Description'][0] : '',
            image: errors?.['Item.ImageFile'] ? errors['Item.ImageFile'][0] : '',
            document: errors?.['Item.DocumentFile'] ? errors['Item.DocumentFile'][0] : '',
            categories: errors?.Categories ? errors.Categories[0] : '',
            color: "meta-1",
          });
        }


      })
    } catch (error) {
      console.error('Error during form submission', error);
    }
  };



  return (
    <form onSubmit={handleSubmit}>
      <p className={`text-${resMessage.color} text-center`}>{resMessage.content}</p>

      {/* title & description */}
      <div className="flex flex-row justify-center items-center">
        {/* title */}
        <div className="w-1/2 mx-2">
          <label className="mb-3 block text-black dark:text-white">Title</label>
          <p className={`text-${resMessage.color}`}>{resMessage.title}</p>

          <input
            type="text"
            placeholder="Title"
            value={formData.item.title}
            onChange={(e) => setFormData((pev) => ({ ...pev, item: { ...pev.item, title: e.target.value }, }))}
            className="w-full rounded-lg border-[1.5px] border-primary bg-transparent py-3 px-5 font-medium outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:bg-form-input"
          />
        </div>

        {/* description */}
        <div className="w-1/2 mx-2">
          <label className="mb-3 block text-black dark:text-white">Description</label>
          <p className={`text-${resMessage.color}`}>{resMessage.description}</p>
          <input
            type="text"
            placeholder="Description"
            value={formData.item.description}
            onChange={(e) => setFormData((pev) => ({ ...pev, item: { ...pev.item, description: e.target.value }, }))}
            className="w-full rounded-lg border-[1.5px] border-primary bg-transparent py-3 px-5 font-medium outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:bg-form-input"
          />
        </div>
      </div>

      {/* image & document */}
      <div className="flex flex-row justify-center items-center mt-5">
        <div className="w-1/2 mx-2">
          <label className="block text-black dark:text-white">
            Image
            <p className="text-center text-meta-3">
              File choosen: {formData.item.imageFile?.name ?? "No file choosen"}
            </p>
            <p className={`text-${resMessage.color}`}>{resMessage.image}</p>
          </label>

          <div className="flex flex-row items-center">
            {item?.image && (
              <img src={item.image} className="w-36 h-36" alt="" />
            )}
            <FileUpload onFileChange={handleImageChange} accept="image/*" />


          </div>
        </div>

        <div className="w-1/2 mx-2">
          <label className="block text-black dark:text-white">
            Document
            <p className="text-center text-meta-3">
              File choosen: {formData.item.documentFile?.name ?? "No file choosen"}
            </p>
            <p className={`text-${resMessage.color}`}>{resMessage.document}</p>
          </label>

          <div className="flex flex-row items-center">
            {/* {item?.image && (
              <img src={item.image} className="w-36 h-36" alt="" />
            )} */}
            <FileUpload onFileChange={handleDocumentChange} />


          </div>
        </div>
      </div>

      {/* Categories */}
      <div>
        <label className="mb-3 block text-black dark:text-white">Categories</label>
        <p className={`text-meta-1`}>{resMessage.categories}</p>

        <MultiSelect
          options={categories.map(cate => {
            return {
              label: cate.categoryName,
              value: cate.categoryId,
              key: cate.description
            }
          })}
          value={selectedCategories}
          onChange={setSelectedCategories}
          labelledBy="Select"
        />
      </div>

      <div className="flex flex-row justify-center items-center mt-5">

        {/* starting price */}
        <div className="w-1/3 mx-2">
          <label className="mb-3 block text-black dark:text-white">Starting Price</label>
          <input
            type="number"
            placeholder="Starting Price"
            value={formData.item.startingPrice}
            onChange={(e) => setFormData((pev) => ({ ...pev, item: { ...pev.item, startingPrice: Number(e.target.value) }, }))}
            className="w-full rounded-lg border-[1.5px] border-primary bg-transparent py-3 px-5 font-medium outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:bg-form-input"
          />
        </div>

        {/* increasing amount */}
        <div className="w-1/3 mx-2">
          <label className="mb-3 block text-black dark:text-white">Increasing Amount</label>
          <p className={`text-${resMessage.color}`}>
            {resMessage.content == "require: IncreasingAmount > 10% StartingPrice" 
            || resMessage.content == "require: IncreasingAmount > $10 when StartingPrice is zero"  && resMessage.content}
          </p>
          <input
            type="number"
            placeholder="Increasing Amount"
            value={formData.item.increasingAmount}
            onChange={(e) => setFormData((pev) => ({ ...pev, item: { ...pev.item, increasingAmount: Number(e.target.value) }, }))}
            className="w-full rounded-lg border-[1.5px] border-primary bg-transparent py-3 px-5 font-medium outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:bg-form-input"
          />
        </div>

        {/* Reserve price */}
        <div className="w-1/3 mx-2">
          <label className="mb-3 block text-black dark:text-white">Reserve Price</label>
          <p className={`text-${resMessage.color}`}>
            {resMessage.content == "require: ReservePrice > StartingPrice" && resMessage.content}
          </p>
          <input
            type="number"
            placeholder="Reserve Price"
            value={formData.item.reservePrice || ''}
            onChange={(e) => setFormData((pev) => ({ ...pev, item: { ...pev.item, reservePrice: Number(e.target.value) }, }))}
            className="w-full rounded-lg border-[1.5px] border-primary bg-transparent py-3 px-5 font-medium outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:bg-form-input"
          />
        </div>  
      </div>

      <div className="flex flex-row justify-center items-center mt-5">

        {/* start date */}
        <div className="w-1/3 mx-2">
          <label className="mb-3 block text-black dark:text-white">Start Date</label>
          <p className={`text-${resMessage.color}`}>
            {resMessage.content == "require: StartDate < EndDate"  && resMessage.content}
          </p>
          <input
            type="datetime-local"
            value={formData.item.startDate.substring(0, 10)} // Extracting the date part
            onChange={(e) => setFormData((prev) => ({ ...prev, item: { ...prev.item, startDate: e.target.value }, }))}
            className="w-full rounded-lg border-[1.5px] border-primary bg-transparent py-3 px-5 font-medium outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:bg-form-input"
          />
        </div>

        {/* end date */}
        <div className="w-1/3 mx-2">
          <label className="mb-3 block text-black dark:text-white">End Date</label>
          <input
            type="date"
            value={formData.item.endDate.substring(0, 10)} // Extracting the date part
            onChange={(e) => setFormData((prev) => ({ ...prev, item: { ...prev.item, endDate: e.target.value }, }))}
            className="w-full rounded-lg border-[1.5px] border-primary bg-transparent py-3 px-5 font-medium outline-none transition focus:border-primary active:border-primary disabled:cursor-default disabled:bg-whiter dark:bg-form-input"
          />
        </div>
      </div>


      {/* Add more fields as needed */}

          <div className="flex w-full flex-row justify-center">
      <button type="submit" className="mt-4 bg-primary text-white py-3 px-6 rounded-lg">
        {!item ? "Create Item" : "Update Item"}
      </button>
          </div>
    </form>
  );
}

export default Index
