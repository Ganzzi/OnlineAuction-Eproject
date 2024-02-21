import { SearchParams } from "@/types/next";
import { ReadonlyURLSearchParams } from "next/navigation";

const parseDate: (dateString: string) => Date = (dateString: string) => {
    return new Date(dateString);
}

const convertSearchParamsToURLSearchParams = (searchParams: SearchParams): URLSearchParams => {
    const convertedParams = new URLSearchParams();
  
    // Iterate over the entries of the SearchParams object
    Object.entries(searchParams).forEach(([key, value]) => {
      // Handle undefined values
      if (value === undefined) {
        convertedParams.delete(key);
      } else if (typeof value === 'string') {
        // Handle string values
        convertedParams.set(key, value);
      } else if (Array.isArray(value)) {
        // Handle array values
        value.forEach(v => {
          convertedParams.append(key, v);
        });
      }
    });
  
    return convertedParams;
  }
  
  const convertToURLSearchParams = (params: ReadonlyURLSearchParams): URLSearchParams => {
    const newParams = new URLSearchParams();
    params.forEach((value, key) => {
      newParams.append(key, value);
    });
    return newParams;
  }

export {parseDate, convertSearchParamsToURLSearchParams, convertToURLSearchParams}